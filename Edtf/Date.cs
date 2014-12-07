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
	public struct Date	{

		public BaseDate StartValue { get; set; }
		public BaseDate EndValue { get; set; }

		/// <summary>
		/// By default, a Date's two values are considered to be an interval, or for the case
		/// where there is no EndValue, a single date. This property is used to specify that
		/// the StartValue and EndValue should be considered a series of dates between them,
		/// inclusive, rather than an interval, which covers the entire time period. If only
		/// StartValue is specified, this becomes an "on or after" date.
		/// If only EndValue is specified, it is "on or before" that date.
		/// </summary>
		/// <value><c>true</c> if this instance is inclusive; otherwise, <c>false</c>.</value>
		public bool IsInclusive { get; set; }

		public override string ToString() {
			var s = StartValue.ToString();
			var e = EndValue.ToString();
			if (IsInclusive)
				return s + ".." + e;
			if (String.IsNullOrEmpty(e))
				return s;
			return s + '/' + e;
		}

		/*
		 * Abandoned for now due to complexity comparing fuzzy BaseDates.
		public ContainsResult Contains(BaseDate item) {
			var rStart = ContainsResult = StartValue.Contains(item);
			if (rStart == ContainsResult.Yes)
				return rStart;
			var rEnd = EndValue.Contains(item);
			if (rEnd = ContainsResult.Yes)
				return rEnd;
			return r;
		}
		*/

		public static Date Parse(string s) {
			var result = new Date();
			if (String.IsNullOrEmpty(s))
				return result;
			string sv = "", ev = "";
			var i = s.IndexOf('/');
			if (i > 0) {
				sv = s.Substring(0, i);
				ev = s.Substring(i + 1);
			} else if (s.StartsWith("..", StringComparison.InvariantCulture)) {
				ev = s.Substring(2);
				result.IsInclusive = true;
			} else if (s.EndsWith("..", StringComparison.InvariantCulture)) {
				sv = s.Substring(0, s.Length - 2);
				result.IsInclusive = true;
			} else {
				i = s.IndexOf("..", StringComparison.InvariantCulture);
				if(i > 0) {
					sv = s.Substring(0, i);
					ev = s.Substring(i + 2);
					result.IsInclusive = true;
				} else {
					sv = s;
				}
			}
			result.StartValue = BaseDate.Parse(sv);
			result.EndValue = BaseDate.Parse(ev);
			return result;
		}

	}

}