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
using System.Collections.Generic;

namespace Edtf {

	public class DatePairList : IList<DatePair>, ICollection<DatePair> {

		/// <summary>
		/// By default, a list of dates is a [set], meaning the true value is unknown but is ONE
		/// of the values in the list. If the mode is set to {multiple}, *every* value is true,
		/// i.e., there is more than one valid value.
		/// </summary>
		private DatePairListMode pvtMode = DatePairListMode.OneOfASet;
		private readonly List<DatePair> pvtList = new List<DatePair>();

		public DatePairListMode Mode { 
			get {
				return pvtMode;
			}
			set {
				pvtMode = value;
			}
		}

		public override string ToString() {
			if (pvtList.Count == 0) return String.Empty;
			var CDL = String.Join(", ", from d in pvtList select d.ToString());
			if (pvtMode == DatePairListMode.Multiple) {
				return '{' + CDL + '}';
			}
			if (pvtList.Count == 1 && !pvtList[0].IsInclusive) {
				return CDL;
			}
			return '[' + CDL + ']';
		}

		public static DatePairList Parse(string s) {
			var result = new DatePairList();
			if (String.IsNullOrEmpty(s)) return result;
			char firstChar = s[0];
			bool isMultiple = (firstChar == '{');
			bool isList = isMultiple || firstChar == '[';
			if (isList) {
				// TODO: Validate that last character is } or ]
				if (isMultiple) result.Mode = DatePairListMode.Multiple;
				result.AddRange(from v in s.Substring(1, s.Length - 2).Split('.') select DatePair.Parse(v.Trim()));
			} else {
				var dSingle = DatePair.Parse(s);
				result.Add(dSingle);
			}
			return result;
		}

		// Convenience function lifted from List<T>, not part of IList
		public void AddRange(IEnumerable<DatePair> list) {
			if (list == null)
				return;
			foreach (var d in list)
				Add(d);  
		}

		#region IList implementation
		public int IndexOf(DatePair item) {
			return pvtList.IndexOf(item);
		}

		public void Insert(int index, DatePair item) {
			pvtList.Insert(index, item);
		}

		public void RemoveAt(int index) {
			pvtList.RemoveAt(index);
		}

		public DatePair this[int index] {
			get {
				return pvtList[index];
			}
			set {
				pvtList[index] = value;
			}
		}
		#endregion

		#region ICollection implementation
		public void Add(DatePair item) {
			pvtList.Add(item);
		}

		public void Clear() {
			pvtList.Clear();
		}

		public bool Contains(DatePair item) {
			return pvtList.Contains(item);
		}

		/* Abandoned for now, complexities in Date comparison
		 * public ContainsResult Contains(Date item) {
			var r = ContainsResult.No;
			foreach (var d in this) {
				var thisR = d.Contains(item);
				if (r != ContainsResult.No) {
					r = thisR;
					if (r == ContainsResult.Yes)
						break;
				}
			}
			return r;
		}
		*/

		public void CopyTo(DatePair[] array, int arrayIndex) {
			pvtList.CopyTo(array, arrayIndex);
		}

		public bool Remove(DatePair item) {
			return pvtList.Remove(item);
		}

		public int Count {
			get {
				return pvtList.Count;
			}
		}

		public bool IsReadOnly {
			get {
				return false;
			}
		}
		#endregion

		#region IEnumerable implementation
		public IEnumerator<DatePair> GetEnumerator() {
			return pvtList.GetEnumerator();
		}
		#endregion

		#region IEnumerable implementation
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return pvtList.GetEnumerator();
		}
		#endregion

	}

}