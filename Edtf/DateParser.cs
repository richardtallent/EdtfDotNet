﻿//
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

	public static class DateParser {

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

		public static Date Parse(string rawValue) {
			var result = new Date();

			if (String.IsNullOrEmpty(rawValue)) {
				result.Status = DateStatus.Unused;
				return result;
			}

			if (rawValue == SpecialValues.Open) {
				result.Status = DateStatus.Open;
				return result;
			}

			if (rawValue == SpecialValues.Unknown) {
				result.Status = DateStatus.Unknown;
				return result;
			}

			var re = Matcher;
			var m = re.Match(rawValue);
			if (!m.Success) {
				result.Status = DateStatus.Invalid;
				return result;
			}

			result.Status = DateStatus.Normal;
			var g = m.Groups;


			// Take the returned regular expression match and parse it into the various date/time bits,
			// validating as needed.

			var YearVal = g["yearnum"].Value;


			// A Year is required.
			if (String.IsNullOrEmpty(YearVal))
				return result;

			// Convert the year, this handles both normal and scientific notation
			// Only parse using a Double first if there is an exponent.
			result.Year = DatePart.Parse(YearVal, true);

			var YearPrecision = g["yearprecision"].Value;
			if (!String.IsNullOrEmpty(YearPrecision)) {
				// The part after "p" are the number of *significant* digits, so
				// to find the insignificant count, convert the value to a temp
				// string to get its length.
				// http://stackoverflow.com/questions/4483886/how-can-i-get-a-count-of-the-total-number-of-digits-in-a-number
				var TotalDigits = Math.Floor(Math.Log10(result.Year.Value) + 1);
				var InsigDigits = TotalDigits - Int32.Parse(YearPrecision);
				result.Year.InsignificantDigits = (InsigDigits < 0) ? (byte)0 : (byte)InsigDigits;
			}

			var YearFlagsVal = g["yearflags"].Value;
			if (!String.IsNullOrEmpty(YearFlagsVal)) {
				result.Year.IsApproximate = YearFlagsVal.Contains('~');
				result.Year.IsUncertain = YearFlagsVal.Contains('?');
			}

			var YearClosesParen = (!String.IsNullOrEmpty(g["yearcloseparen"].Value));

			var MonthVal = g["monthnum"].Value;
			if (String.IsNullOrEmpty(MonthVal)) return result;
			result.Month = DatePart.Parse(MonthVal, false);

			var MonthOpensParen = (g["month"].Value[0] == '(');
			var MonthClosesParen = (!String.IsNullOrEmpty(g["monthcloseparen"].Value));

			var MonthFlagsVal = g["monthflags"].Value;
			if (!String.IsNullOrEmpty(MonthFlagsVal)) {
				result.Month.IsApproximate = MonthFlagsVal.Contains('~');
				result.Month.IsUncertain = MonthFlagsVal.Contains('?');
				if (!MonthOpensParen && !YearClosesParen) {
					result.Year.IsApproximate |= result.Month.IsApproximate;
					result.Year.IsUncertain |= result.Month.IsUncertain;
				}
			}

			if (result.Month.Value >= 20) {
				result.SeasonQualifier = g["seasonqualifier"].Value;
				// There won't be a day or time, or if there is, it should be ignored
				return result;
			}

			var DayVal = g["daynum"].Value;
			if (String.IsNullOrEmpty(DayVal)) return result;
			result.Day = DatePart.Parse(DayVal, false);

			var DayFlagsVal = g["dayflags"].Value;
			if (!String.IsNullOrEmpty(DayFlagsVal)) {

				result.Day.IsApproximate = DayFlagsVal.Contains('~');
				result.Day.IsUncertain = DayFlagsVal.Contains('?');

				// Needs a rewrite to handle (2004-(06)~)?, which closes the month twice for different purposes. Could happen at the day level as well.
				var DayOpensParen = (g["day"].Value[0]=='(');
				if (!DayOpensParen && !MonthClosesParen) {
					result.Month.IsApproximate |= result.Day.IsApproximate;
					result.Month.IsUncertain |= result.Day.IsUncertain;
					if (!YearClosesParen && !MonthOpensParen) {
						result.Year.IsApproximate |= result.Day.IsApproximate;
						result.Year.IsUncertain |= result.Day.IsUncertain;
					}
				}

			}

			// TIME

			var hourVal = g["hour"].Value;
			if (String.IsNullOrEmpty(hourVal))
				return result;
			result.Hour = Int32.Parse(hourVal);
			result.Minute = Int32.Parse(g["minute"].Value);
			result.Second = Int32.Parse(g["second"].Value);

			// Time zone offset
			var tzSignValue = g["tzsign"].Value;
			if (!String.IsNullOrEmpty(tzSignValue)) {
				var tzSign = (tzSignValue == "-") ? -1 : 1;
				var tzHour = Int32.Parse(g["tzhour"].Value);
				var tzMinute = Int32.Parse(g["tzminute"].Value);
				result.TimeZoneOffset = tzSign * (tzHour * 60) + tzMinute;
			}

			return result;

		}


	
	}
}