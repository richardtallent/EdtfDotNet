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
using System.Globalization;

namespace Edtf {

	/// <summary>
	/// 
	/// FuzzyInt is a structure that includes both an integer value and flags to convey
	/// whether there is a value, if it is uncertain and/or approximate, which digits
	/// were not specified, and how precise the number is.
	/// 
	/// This structure forms the basis for each of the terms (year, month, etc.) in an
	/// EDTF Date/Time value. Implicit conversion to and from a string is supported,
	/// and string parsing from EDTF form is the expected means of construction. If an
	/// issue occurs during the string parsing, the value will have a Status of Invalid. 
	///
	/// The ~, and ? tokens can apply to more than one portion of a date, so the individual
	/// struct value may need to be manipulated further after the initial parsing. However,
	/// there is explicitly no support in this implementation for parenthetical grouping
	/// to specify the scope of these tokens (those are unsupported Level 2 features).
	/// 
	/// This is a very tight structure, the total size is 4 integers. It cannot store
	/// dates further back than the mid-Rhyacian period (2.147 billion years ago).
	/// 
	/// </summary>
	public struct FuzzyInt {

		private int pvtValue;
		public int Value {
			get {
				return pvtValue;
			}
			set {
				pvtValue = value;
				HasValue = true;
			}
		}

		public bool Invalid { get; set; }
		public bool HasValue { get; set; }
		public bool IsUncertain { get; set; }
		public bool IsApproximate { get; set; }

		public int UnspecifiedMask { get; set; }			// places with a 1 are emitted as "u"
		public int FirstPreciseDigitPlace { get; set; }	// 1 for year, 2 for decade, etc.

		public override string ToString() {
			return ToString(0, false, false);
		}

		public string ToString(int padDigits, bool alreadyUncertain, bool alreadyApproximate) {

			if (!HasValue) return String.Empty;

			var u = UnspecifiedMask.ToString(CultureInfo.InvariantCulture);

			// Get the absolute value because we may need to mask the "u" or "x" positions
			// Pad it on the left so it has enough characters for the unspecified mask, and
			// convert it to an array so we can manipulate characters individually.
			var v = Math.Abs(Value).ToString(CultureInfo.InvariantCulture).PadLeft(u.Length, 'u').ToCharArray();

			// If the precision is not 1, set digits to the right of it as "x"
			for (int x = 1; x < FirstPreciseDigitPlace; x++) {
				v[v.Length - x] = 'x';
			}

			// Set any unspecified digits
			for (int i = 1; i < u.Length; i++) {
				if (u[u.Length - i] == '1')
					v[v.Length - i] = 'u';
			}

			var v2 = new string(v).PadLeft(padDigits, '0');

			if (Value < 0) {
				v2 = '-' + v2;
			}

			if(Value > 9999 || Value < -9999) {
				// this is more than a 4-digit year, add the "y" prefix (it's optional, but probably wise in these exceptional cases)
				v2 = 'y' + v2;
			}

			if (IsUncertain && !alreadyUncertain)
				v2 += '?';
			if (IsApproximate && !alreadyApproximate)
				v2 += '~';

			return v2;

		}

	}

}