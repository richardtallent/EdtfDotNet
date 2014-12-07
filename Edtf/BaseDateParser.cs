//
// Enums.cs
//
// Author:
//       Richard S. Tallent, II <richard@tallent.us>
//
// Copyright (c) 2014 Richard S. Tallent, II
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Edtf {

	public static class BaseDateParser {

		private static Regex _matcher;
		private static object matchLoadLocker = new object();
		private static Regex Matcher {
			get {
				if (_matcher == null) {
					lock (matchLoadLocker) {
						if (_matcher == null) {
							var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Edtf.EdtfRegexPattern.txt");
							var pattern = "";
							using (var reader = new System.IO.StreamReader (stream)) {
								pattern = reader.ReadToEnd ();
							}
							_matcher = new Regex(pattern, RegexOptions.Compiled);
						}
					}
				}
				return _matcher;
			}
		}

		private static int GetMask(string s, char mc) {
			var arr = (from c in s
			           select (c == mc ? '1' : '0')).ToArray();
			return Int32.Parse(new string(arr));
		}

		public static BaseDate Parse(string rawValue) {
			var result = new BaseDate();

			if (String.IsNullOrEmpty(rawValue)) {
				result.Status = BaseDateStatus.Unused;
				return result;
			}

			if (rawValue == "open") {
				result.Status = BaseDateStatus.Open;
				return result;
			}

			if (rawValue == "unknown") {
				result.Status = BaseDateStatus.Unknown;
				return result;
			}

			var re = Matcher;
			var m = re.Match(rawValue);
			if (!m.Success) {
				result.Status = BaseDateStatus.Invalid;
				return result;
			}

			result.Status = BaseDateStatus.Normal;

			// Take the returned regular expression match and parse it into the various date/time bits,
			// validating as needed.

			var YearVal = m.Groups["yearnum"].Value;

			// A Year is required.
			if (String.IsNullOrEmpty(YearVal))
				return result;

			// Convert the year, this handles both normal and scientific notation
			result.Year.Value = Convert.ToInt32(Single.Parse(YearVal.Replace('u', '0').Replace('x', '0')));
			result.Year.UnspecifiedMask = GetMask(YearVal, 'u');
			var YearPrecision = m.Groups["yearprecision"].Value;
			if (String.IsNullOrEmpty(YearPrecision)) {
				result.Year.FirstPreciseDigitPlace = (YearVal.Count(t => t == 'x')) + 1;
			} else {
				// TODO: Not in the same terms
				result.Year.FirstPreciseDigitPlace = Byte.Parse(YearPrecision);
			}
			var YearFlagsVal = m.Groups["yearflags"].Value;
			if (!String.IsNullOrEmpty(YearFlagsVal)) {
				result.Year.IsApproximate = YearFlagsVal.Contains('~');
				result.Year.IsUncertain = YearFlagsVal.Contains('?');
			}

			var MonthVal = m.Groups["monthnum"].Value;
			if (String.IsNullOrEmpty(MonthVal))
				return result;
			result.Month.Value = Int32.Parse(MonthVal.Replace('u', '0'));
			result.Month.UnspecifiedMask = GetMask(MonthVal, 'u');

			if (result.Month.Value >= 20) {
				result.SeasonQualifier = m.Groups["seasonqualifier"].Value;
				// There won't be a day or time, or if there is, it should be ignored
				return result;
			}

			var MonthFlagsVal = m.Groups["monthflags"].Value;
			if (!String.IsNullOrEmpty(MonthFlagsVal)) {
				var PropogatesToYear = String.IsNullOrEmpty(m.Groups["monthcloseparen"].Value) || m.Groups["month"].Value[0]!='(';
				result.Month.IsApproximate = MonthFlagsVal.Contains('~');
				result.Month.IsUncertain = MonthFlagsVal.Contains('?');
				if (PropogatesToYear) {
					result.Year.IsApproximate |= result.Month.IsApproximate;
					result.Year.IsUncertain |= result.Month.IsUncertain;
				}
			}

			var DayVal = m.Groups["daynum"].Value;
			if (String.IsNullOrEmpty(DayVal))
				return result;
			result.Day.Value = Int32.Parse(DayVal.Replace('u', '0'));
			result.Day.UnspecifiedMask = GetMask(DayVal, 'u');

			var DayFlagsVal = m.Groups["dayflags"].Value;
			if (!String.IsNullOrEmpty(DayFlagsVal)) {
				var onlyLocal = m.Groups["day"].Value[0]=='(';
				result.Day.IsApproximate = DayFlagsVal.Contains('~');
				result.Day.IsUncertain = DayFlagsVal.Contains('?');
				if (!onlyLocal) {
					result.Month.IsApproximate |= result.Day.IsApproximate;
					result.Month.IsUncertain |= result.Day.IsUncertain;
					if ( (m.Groups["month"].Value[0]!='(') || (m.Groups["monthcloseparen"].Value == ")") ) {
						result.Year.IsApproximate |= result.Day.IsApproximate;
						result.Year.IsUncertain |= result.Day.IsUncertain;
					}
				}
			}

			// TIME

			var hourVal = m.Groups["hour"].Value;
			if (String.IsNullOrEmpty(hourVal))
				return result;
			result.Hour = Int32.Parse(hourVal);
			result.Minute = Int32.Parse(m.Groups["minute"].Value);
			result.Second = Int32.Parse(m.Groups["second"].Value);

			// Time zone offset
			var tzSignValue = m.Groups["tzsign"].Value;
			if (!String.IsNullOrEmpty(tzSignValue)) {
				var tzSign = (tzSignValue == "-") ? -1 : 1;
				var tzHour = Int32.Parse(m.Groups["tzhour"].Value);
				var tzMinute = Int32.Parse(m.Groups["tzminute"].Value);
				result.TimeZoneOffset = tzSign * (tzHour * 60) + tzMinute;
			}

			return result;

		}
	
	}
}