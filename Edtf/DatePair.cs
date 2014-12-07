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

namespace Edtf {

	/// <summary>
	/// An ETDF "Date" actually consists of a starting and ending value and a Mode to describe
	/// how they are related. Both values are optional, but at least one should be provided.
	/// 
	/// TODO: Comparison, for reasonable sorting of dates
	/// 
	/// </summary>
	public struct DatePair	{

		public Date StartValue { get; set; }
		public Date EndValue { get; set; }

		/// <summary>
		/// By default, a Date's two values are considered to be an Interval (every possible instant
		/// between the StartValue and EndValue, or for the case where there is no EndValue, a single
		/// date). But if IsRange is true, this DatePair includes all *discrete* values between the
		/// StartValue and EndValue using the most precise of the StartValue and EndValue values. For
		/// example, 2008..2010 represents the range of 2008, 2009, 2010, but would not precisely
		/// match the date 2008-03-22, whereas 2008/2010 would. IsRange can be used with only one of
		/// the two values -- if only StartValue is specified, this becomes an "on or after" date.
		/// If only EndValue is specified, it is "on or before" that date.
		/// </summary>
		/// <value><c>true</c> if this instance is a range of discrete; <c>false</c>.</value> if it
		/// is a single date value or represents the entire interval of time between the values.
		public bool IsRange { get; set; }

		private const char IntervalDelimiter = '/';
		private const string RangeDelimiter = "..";

		public override string ToString() {
			var s = StartValue.ToString();
			var e = EndValue.ToString();
			if (IsRange)
				return s + RangeDelimiter + e;
			if (String.IsNullOrEmpty(e))
				return s;
			return s + IntervalDelimiter + e;
		}

		/*
		 * Abandoned for now due to complexity comparing fuzzy Dates.
		public ContainsResult Contains(Date item) {
			var rStart = ContainsResult = StartValue.Contains(item);
			if (rStart == ContainsResult.Yes)
				return rStart;
			var rEnd = EndValue.Contains(item);
			if (rEnd = ContainsResult.Yes)
				return rEnd;
			return r;
		}
		*/

		public static DatePair Parse(string s) {
			var result = new DatePair();
			string sv, ev;
			if (String.IsNullOrEmpty(s)) {
				sv = String.Empty;
				ev = String.Empty;
			} else {
				var i = s.IndexOf(IntervalDelimiter);
				if (i > 0) {
					// The interval can't start with "/", hence the > instead of >=
					sv = s.Substring(0, i);
					ev = s.Substring(i + 1);
				} else { 
					var j = s.IndexOf(RangeDelimiter, StringComparison.InvariantCulture);
					if (j < 0) {
						// This is not an interval or range, just a single value.
						sv = s;
						ev = String.Empty;
					} else {
						// Covers sv..ev, ..ev, and sv..
						sv = s.Substring(0, i);
						ev = s.Substring(i + RangeDelimiter.Length);
						result.IsRange = true;
					}
				}
			}
			result.StartValue = Date.Parse(sv);
			result.EndValue = Date.Parse(ev);
			return result;
		}

	}

}