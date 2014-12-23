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

using NUnit.Framework;
using Edtf;

namespace EdtfTests {

	/// <summary>
	/// This test suite uses the examples in the Features table of the EDTF 1.0 Draft Submission, available here:
	/// http://www.loc.gov/standards/datetime/pre-submission.html#table
	/// </summary>

	#region Level 0. ISO 8601 Features

	[TestFixture()] public class TestL0Date {

		[Test] public void TestL0DateComplete() {
			const string DateString = "2001-02-03";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(2001, TestDate.StartValue.Year.Value);
			Assert.AreEqual(2, TestDate.StartValue.Month.Value);
			Assert.AreEqual(3, TestDate.StartValue.Day.Value);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL0DateMonth() {
			const string DateString = "2008-12";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(2008, TestDate.StartValue.Year.Value);
			Assert.AreEqual(12, TestDate.StartValue.Month.Value);
			Assert.AreEqual(false, TestDate.StartValue.Day.HasValue);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL0DateYear() {
			const string DateString = "2008";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(2008, TestDate.StartValue.Year.Value);
			Assert.AreEqual(false, TestDate.StartValue.Month.HasValue);
			Assert.AreEqual(false, TestDate.StartValue.Day.HasValue);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL0DateNegativeYear() {
			const string DateString = "-0999";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(-999, TestDate.StartValue.Year.Value);
			Assert.AreEqual(false, TestDate.StartValue.Month.HasValue);
			Assert.AreEqual(false, TestDate.StartValue.Day.HasValue);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL0DateYear0() {
			const string DateString = "0000";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(0, TestDate.StartValue.Year.Value);
			Assert.AreEqual(true, TestDate.StartValue.Year.HasValue);
			Assert.AreEqual(false, TestDate.StartValue.Month.HasValue);
			Assert.AreEqual(false, TestDate.StartValue.Day.HasValue);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}
	}

	[TestFixture()] public class TestL0DateAndTime {

		[Test] public void TestL0DateTime1() {
			const string DateString = "2001-02-03T09:30:01";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(2001, TestDate.StartValue.Year.Value);
			Assert.AreEqual(2, TestDate.StartValue.Month.Value);
			Assert.AreEqual(3, TestDate.StartValue.Day.Value);
			Assert.AreEqual(9, TestDate.StartValue.Hour);
			Assert.AreEqual(30, TestDate.StartValue.Minute);
			Assert.AreEqual(1, TestDate.StartValue.Second);
			Assert.AreEqual(0, TestDate.StartValue.TimeZoneOffset);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			// TODO: This fails because the spec does not specify the meaning of the lack of a TZ offset.
			// If the meaning is a presumption of UTC, this test should be modified. If the meaning is that
			// the TZ is unknown and should *not* be assumed to be UTC, the code needs to be modified to
			// track the presence/absence of a 0-value TZ offset.
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL0DateTime2() {
			const string DateString = "2004-01-01T10:10:10Z";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(2004, TestDate.StartValue.Year.Value);
			Assert.AreEqual(1, TestDate.StartValue.Month.Value);
			Assert.AreEqual(1, TestDate.StartValue.Day.Value);
			Assert.AreEqual(10, TestDate.StartValue.Hour);
			Assert.AreEqual(10, TestDate.StartValue.Minute);
			Assert.AreEqual(10, TestDate.StartValue.Second);
			Assert.AreEqual(0, TestDate.StartValue.TimeZoneOffset);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL0DateTime3() {
			const string DateString = "2004-01-01T10:10:10+05:00";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(2004, TestDate.StartValue.Year.Value);
			Assert.AreEqual(1, TestDate.StartValue.Month.Value);
			Assert.AreEqual(1, TestDate.StartValue.Day.Value);
			Assert.AreEqual(10, TestDate.StartValue.Hour);
			Assert.AreEqual(10, TestDate.StartValue.Minute);
			Assert.AreEqual(10, TestDate.StartValue.Second);
			Assert.AreEqual(300, TestDate.StartValue.TimeZoneOffset);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}
	}

	[TestFixture()] public class TestL0Interval {

		[Test] public void TestL0Interval1() {
			const string DateString = "1964/2008";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(1964, TestDate.StartValue.Year.Value);	
			Assert.AreEqual(2008, TestDate.EndValue.Year.Value);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Normal, TestDate.EndValue.Status);
			Assert.AreEqual(false, TestDate.IsRange);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL0Interval2() {
			const string DateString = "2004-06/2006-08";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(2004, TestDate.StartValue.Year.Value);	
			Assert.AreEqual(2006, TestDate.EndValue.Year.Value);
			Assert.AreEqual(6, TestDate.StartValue.Month.Value);	
			Assert.AreEqual(8, TestDate.EndValue.Month.Value);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Normal, TestDate.EndValue.Status);
			Assert.AreEqual(false, TestDate.IsRange);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL0Interval3() {
			const string DateString = "2004-02-01/2005-02-08";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(2004, TestDate.StartValue.Year.Value);	
			Assert.AreEqual(2005, TestDate.EndValue.Year.Value);
			Assert.AreEqual(2, TestDate.StartValue.Month.Value);	
			Assert.AreEqual(2, TestDate.EndValue.Month.Value);
			Assert.AreEqual(1, TestDate.StartValue.Day.Value);	
			Assert.AreEqual(8, TestDate.EndValue.Day.Value);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Normal, TestDate.EndValue.Status);
			Assert.AreEqual(false, TestDate.IsRange);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL0Interval4() {
			const string DateString = "2004-02-01/2005-02";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(2004, TestDate.StartValue.Year.Value);	
			Assert.AreEqual(2005, TestDate.EndValue.Year.Value);
			Assert.AreEqual(2, TestDate.StartValue.Month.Value);	
			Assert.AreEqual(2, TestDate.EndValue.Month.Value);
			Assert.AreEqual(1, TestDate.StartValue.Day.Value);	
			Assert.AreEqual(false, TestDate.EndValue.Day.HasValue);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Normal, TestDate.EndValue.Status);
			Assert.AreEqual(false, TestDate.IsRange);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL0Interval5() {
			const string DateString = "2004-02-01/2005";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(2004, TestDate.StartValue.Year.Value);	
			Assert.AreEqual(2005, TestDate.EndValue.Year.Value);
			Assert.AreEqual(2, TestDate.StartValue.Month.Value);	
			Assert.AreEqual(false, TestDate.EndValue.Month.HasValue);
			Assert.AreEqual(1, TestDate.StartValue.Day.Value);	
			Assert.AreEqual(false, TestDate.EndValue.Day.HasValue);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Normal, TestDate.EndValue.Status);
			Assert.AreEqual(false, TestDate.IsRange);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL0Interval6() {
			const string DateString = "2005/2006-02";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(2005, TestDate.StartValue.Year.Value);	
			Assert.AreEqual(2006, TestDate.EndValue.Year.Value);
			Assert.AreEqual(false, TestDate.StartValue.Month.HasValue);	
			Assert.AreEqual(2, TestDate.EndValue.Month.Value);
			Assert.AreEqual(false, TestDate.StartValue.Day.HasValue);	
			Assert.AreEqual(false, TestDate.EndValue.Day.HasValue);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Normal, TestDate.EndValue.Status);
			Assert.AreEqual(false, TestDate.IsRange);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

	}

	#endregion

	#region Level 1 Extensions

	[TestFixture()] public class TestL1UncertainApprox {

		[Test] public void TestL1Uncertain1() {
			const string DateString = "1984?";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(1984, TestDate.StartValue.Year.Value);
			Assert.AreEqual(true, TestDate.StartValue.Year.IsUncertain);
			Assert.AreEqual(false, TestDate.StartValue.Year.IsApproximate);
			Assert.AreEqual(false, TestDate.StartValue.Month.HasValue);
			Assert.AreEqual(false, TestDate.StartValue.Month.IsUncertain);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL1Uncertain2() {
			const string DateString = "2004-06?";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(2004, TestDate.StartValue.Year.Value);
			Assert.AreEqual(6, TestDate.StartValue.Month.Value);
			Assert.AreEqual(true, TestDate.StartValue.Year.IsUncertain);
			Assert.AreEqual(0, TestDate.StartValue.Month.UnspecifiedMask);
			Assert.AreEqual(true, TestDate.StartValue.Month.IsUncertain);
			Assert.AreEqual(false, TestDate.StartValue.Day.HasValue);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL1Uncertain3() {
			const string DateString = "2004-06-11?";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(2004, TestDate.StartValue.Year.Value);
			Assert.AreEqual(6, TestDate.StartValue.Month.Value);
			Assert.AreEqual(11, TestDate.StartValue.Day.Value);
			Assert.AreEqual(true, TestDate.StartValue.Day.IsUncertain);
			Assert.AreEqual(true, TestDate.StartValue.Month.IsUncertain);
			Assert.AreEqual(true, TestDate.StartValue.Year.IsUncertain);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL1Approx1() {
			const string DateString = "1984~";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(1984, TestDate.StartValue.Year.Value);
			Assert.AreEqual(false, TestDate.StartValue.Year.IsUncertain);
			Assert.AreEqual(true, TestDate.StartValue.Year.IsApproximate);
			Assert.AreEqual(false, TestDate.StartValue.Month.HasValue);
			Assert.AreEqual(false, TestDate.StartValue.Month.IsApproximate);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL1Approx2() {
			const string DateString = "1984?~";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(1984, TestDate.StartValue.Year.Value);
			Assert.AreEqual(true, TestDate.StartValue.Year.IsUncertain);
			Assert.AreEqual(true, TestDate.StartValue.Year.IsApproximate);
			Assert.AreEqual(false, TestDate.StartValue.Month.HasValue);
			Assert.AreEqual(false, TestDate.StartValue.Month.IsApproximate);
			Assert.AreEqual(false, TestDate.StartValue.Month.IsUncertain);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

	}

	[TestFixture()] public class TestL1Unspecified {

		[Test] public void TestL1Unspecified1() {
			const string DateString = "199u";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(0001, TestDate.StartValue.Year.UnspecifiedMask);
			Assert.AreEqual(1990, TestDate.StartValue.Year.Value);
			Assert.AreEqual(false, TestDate.StartValue.Year.IsApproximate);
			Assert.AreEqual(true, TestDate.StartValue.Year.HasValue);
			Assert.AreEqual(false, TestDate.StartValue.Month.HasValue);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL1Unspecified2() {
			const string DateString = "19uu";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(0011, TestDate.StartValue.Year.UnspecifiedMask);
			Assert.AreEqual(1900, TestDate.StartValue.Year.Value);
			Assert.AreEqual(false, TestDate.StartValue.Year.IsApproximate);
			Assert.AreEqual(true, TestDate.StartValue.Year.HasValue);
			Assert.AreEqual(false, TestDate.StartValue.Month.HasValue);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL1Unspecified3() {
			const string DateString = "1999-uu";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(1999, TestDate.StartValue.Year.Value);
			Assert.AreEqual(0, TestDate.StartValue.Year.UnspecifiedMask);
			Assert.AreEqual(11, TestDate.StartValue.Month.UnspecifiedMask);
			Assert.AreEqual(false, TestDate.StartValue.Year.IsApproximate);
			Assert.AreEqual(true, TestDate.StartValue.Year.HasValue);
			Assert.AreEqual(true, TestDate.StartValue.Month.HasValue);
			Assert.AreEqual(0, TestDate.StartValue.Month.Value);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL1Unspecified4() {
			const string DateString = "1999-01-uu";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(1999, TestDate.StartValue.Year.Value);
			Assert.AreEqual(0, TestDate.StartValue.Year.UnspecifiedMask);
			Assert.AreEqual(0, TestDate.StartValue.Month.UnspecifiedMask);
			Assert.AreEqual(11, TestDate.StartValue.Day.UnspecifiedMask);
			Assert.AreEqual(false, TestDate.StartValue.Year.IsApproximate);
			Assert.AreEqual(true, TestDate.StartValue.Year.HasValue);
			Assert.AreEqual(true, TestDate.StartValue.Month.HasValue);
			Assert.AreEqual(true, TestDate.StartValue.Day.HasValue);
			Assert.AreEqual(1, TestDate.StartValue.Month.Value);
			Assert.AreEqual(0, TestDate.StartValue.Day.Value);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL1Unspecified5() {
			const string DateString = "1999-uu-uu";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(1999, TestDate.StartValue.Year.Value);
			Assert.AreEqual(0, TestDate.StartValue.Year.UnspecifiedMask);
			Assert.AreEqual(11, TestDate.StartValue.Month.UnspecifiedMask);
			Assert.AreEqual(11, TestDate.StartValue.Day.UnspecifiedMask);
			Assert.AreEqual(false, TestDate.StartValue.Year.IsApproximate);
			Assert.AreEqual(true, TestDate.StartValue.Year.HasValue);
			Assert.AreEqual(true, TestDate.StartValue.Month.HasValue);
			Assert.AreEqual(true, TestDate.StartValue.Day.HasValue);
			Assert.AreEqual(0, TestDate.StartValue.Month.Value);
			Assert.AreEqual(0, TestDate.StartValue.Day.Value);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

	}

	[TestFixture()] public class TestL1ExtendedInterval {

		[Test] public void TestL1ExtendedInterval1() {
			const string DateString = "unknown/2006";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(2006, TestDate.EndValue.Year.Value);
			Assert.AreEqual(false, TestDate.StartValue.Year.HasValue);
			Assert.AreEqual(true, TestDate.EndValue.Year.HasValue);
			Assert.AreEqual(false, TestDate.EndValue.Month.HasValue);
			Assert.AreEqual(DateStatus.Unknown, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Normal, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL1ExtendedInterval2() {
			const string DateString = "2004-06-01/unknown";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(2004, TestDate.StartValue.Year.Value);
			Assert.AreEqual(false, TestDate.EndValue.Year.HasValue);
			Assert.AreEqual(true, TestDate.StartValue.Year.HasValue);
			Assert.AreEqual(false, TestDate.EndValue.Month.HasValue);
			Assert.AreEqual(6, TestDate.StartValue.Month.Value);
			Assert.AreEqual(1, TestDate.StartValue.Day.Value);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Unknown, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL1ExtendedInterval3() {
			const string DateString = "2004-01-01/open";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(2004, TestDate.StartValue.Year.Value);
			Assert.AreEqual(false, TestDate.EndValue.Year.HasValue);
			Assert.AreEqual(true, TestDate.StartValue.Year.HasValue);
			Assert.AreEqual(false, TestDate.EndValue.Month.HasValue);
			Assert.AreEqual(1, TestDate.StartValue.Month.Value);
			Assert.AreEqual(1, TestDate.StartValue.Day.Value);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Open, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL1ExtendedInterval4() {
			const string DateString = "1984~/2004-06";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(1984, TestDate.StartValue.Year.Value);
			Assert.AreEqual(true, TestDate.StartValue.Year.IsApproximate);
			Assert.AreEqual(2004, TestDate.EndValue.Year.Value);
			Assert.AreEqual(true, TestDate.EndValue.Year.HasValue);
			Assert.AreEqual(true, TestDate.StartValue.Year.HasValue);
			Assert.AreEqual(true, TestDate.EndValue.Month.HasValue);
			Assert.AreEqual(6, TestDate.EndValue.Month.Value);
			Assert.AreEqual(0, TestDate.EndValue.Day.Value);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Normal, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL1ExtendedInterval5() {
			const string DateString = "1984/2004-06~";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(1984, TestDate.StartValue.Year.Value);
			Assert.AreEqual(false, TestDate.StartValue.Year.IsApproximate);
			Assert.AreEqual(2004, TestDate.EndValue.Year.Value);
			Assert.AreEqual(true, TestDate.EndValue.Year.IsApproximate);
			Assert.AreEqual(true, TestDate.EndValue.Month.IsApproximate);
			Assert.AreEqual(true, TestDate.StartValue.Year.HasValue);
			Assert.AreEqual(true, TestDate.EndValue.Month.HasValue);
			Assert.AreEqual(6, TestDate.EndValue.Month.Value);
			Assert.AreEqual(0, TestDate.EndValue.Day.Value);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Normal, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL1ExtendedInterval6() {
			const string DateString = "1984~/2004~";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(1984, TestDate.StartValue.Year.Value);
			Assert.AreEqual(true, TestDate.StartValue.Year.IsApproximate);
			Assert.AreEqual(2004, TestDate.EndValue.Year.Value);
			Assert.AreEqual(true, TestDate.EndValue.Year.IsApproximate);
			Assert.AreEqual(false, TestDate.EndValue.Month.IsApproximate);
			Assert.AreEqual(true, TestDate.StartValue.Year.HasValue);
			Assert.AreEqual(true, TestDate.EndValue.Year.HasValue);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Normal, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL1ExtendedInterval7() {
			const string DateString = "1984?/2004?~";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(1984, TestDate.StartValue.Year.Value);
			Assert.AreEqual(true, TestDate.StartValue.Year.IsUncertain);
			Assert.AreEqual(false, TestDate.StartValue.Year.IsApproximate);
			Assert.AreEqual(2004, TestDate.EndValue.Year.Value);
			Assert.AreEqual(true, TestDate.EndValue.Year.IsUncertain);
			Assert.AreEqual(true, TestDate.EndValue.Year.IsApproximate);
			Assert.AreEqual(false, TestDate.EndValue.Month.IsApproximate);
			Assert.AreEqual(true, TestDate.StartValue.Year.HasValue);
			Assert.AreEqual(true, TestDate.EndValue.Year.HasValue);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Normal, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL1ExtendedInterval8() {
			const string DateString = "1984-06?/2004-08?";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(1984, TestDate.StartValue.Year.Value);
			Assert.AreEqual(true, TestDate.StartValue.Year.IsUncertain);
			Assert.AreEqual(false, TestDate.StartValue.Year.IsApproximate);
			Assert.AreEqual(2004, TestDate.EndValue.Year.Value);
			Assert.AreEqual(true, TestDate.EndValue.Year.IsUncertain);
			Assert.AreEqual(false, TestDate.EndValue.Year.IsApproximate);
			Assert.AreEqual(true, TestDate.EndValue.Month.IsUncertain);
			Assert.AreEqual(true, TestDate.StartValue.Year.HasValue);
			Assert.AreEqual(true, TestDate.EndValue.Year.HasValue);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Normal, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL1ExtendedInterval9() {
			const string DateString = "1984-06-02?/2004-08-08~";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(1984, TestDate.StartValue.Year.Value);
			Assert.AreEqual(true, TestDate.StartValue.Year.IsUncertain);
			Assert.AreEqual(true, TestDate.StartValue.Month.IsUncertain);
			Assert.AreEqual(true, TestDate.StartValue.Day.IsUncertain);
			Assert.AreEqual(false, TestDate.StartValue.Year.IsApproximate);
			Assert.AreEqual(2004, TestDate.EndValue.Year.Value);
			Assert.AreEqual(8, TestDate.EndValue.Month.Value);
			Assert.AreEqual(8, TestDate.EndValue.Day.Value);
			Assert.AreEqual(true, TestDate.EndValue.Year.IsApproximate);
			Assert.AreEqual(true, TestDate.EndValue.Month.IsApproximate);
			Assert.AreEqual(true, TestDate.EndValue.Day.IsApproximate);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Normal, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL1ExtendedInterval10() {
			const string DateString = "1984-06-02?/unknown";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(1984, TestDate.StartValue.Year.Value);
			Assert.AreEqual(true, TestDate.StartValue.Year.IsUncertain);
			Assert.AreEqual(false, TestDate.StartValue.Year.IsApproximate);
			Assert.AreEqual(true, TestDate.StartValue.Day.IsUncertain);
			Assert.AreEqual(0, TestDate.EndValue.Year.Value);
			Assert.AreEqual(6, TestDate.StartValue.Month.Value);
			Assert.AreEqual(2, TestDate.StartValue.Day.Value);
			Assert.AreEqual(false, TestDate.EndValue.Year.HasValue);
			Assert.AreEqual(false, TestDate.EndValue.Year.IsUncertain);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Unknown, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

	}
		
	[TestFixture()] public class TestL1LongYear {

		[Test] public void TestL1LongYear1() {
			const string DateString = "y170000002";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(170000002, TestDate.StartValue.Year.Value);
			Assert.AreEqual(false, TestDate.StartValue.Year.IsUncertain);
			Assert.AreEqual(false, TestDate.StartValue.Year.IsApproximate);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL1LongYear2() {
			const string DateString = "y-170000002";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(-170000002, TestDate.StartValue.Year.Value);
			Assert.AreEqual(false, TestDate.StartValue.Year.IsUncertain);
			Assert.AreEqual(false, TestDate.StartValue.Year.IsApproximate);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

	}

	[TestFixture()] public class TestL1Season {

		[Test] public void TestL1Season1() {
			const string DateString = "2001-21";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(2001, TestDate.StartValue.Year.Value);
			Assert.AreEqual(Edtf.Seasons.Spring, TestDate.StartValue.Month.Value);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL1Season2() {
			const string DateString = "2003-22";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(2003, TestDate.StartValue.Year.Value);
			Assert.AreEqual(Edtf.Seasons.Summer, TestDate.StartValue.Month.Value);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL1Season3() {
			const string DateString = "2000-23";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(2000, TestDate.StartValue.Year.Value);
			Assert.AreEqual(Edtf.Seasons.Autumn, TestDate.StartValue.Month.Value);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL1Season4() {
			const string DateString = "2010-24";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(2010, TestDate.StartValue.Year.Value);
			Assert.AreEqual(Edtf.Seasons.Winter, TestDate.StartValue.Month.Value);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

	}


	#endregion

	#region Level 2 Extensions

	[TestFixture()] public class TestL2PartialUncertainApprox {

		[Test] public void TestL2PartialUncertainApprox1() {
			const string DateString = "2004?-06-11";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(2004, TestDate.StartValue.Year.Value);
			Assert.AreEqual(true, TestDate.StartValue.Year.IsUncertain);
			Assert.AreEqual(false, TestDate.StartValue.Month.IsUncertain);
			Assert.AreEqual(false, TestDate.StartValue.Day.IsUncertain);
			Assert.AreEqual(6, TestDate.StartValue.Month.Value);
			Assert.AreEqual(11, TestDate.StartValue.Day.Value);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL2PartialUncertainApprox2() {
			const string DateString = "2004-06~-11";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(2004, TestDate.StartValue.Year.Value);
			Assert.AreEqual(true, TestDate.StartValue.Year.IsApproximate);
			Assert.AreEqual(true, TestDate.StartValue.Month.IsApproximate);
			Assert.AreEqual(false, TestDate.StartValue.Day.IsApproximate);
			Assert.AreEqual(6, TestDate.StartValue.Month.Value);
			Assert.AreEqual(11, TestDate.StartValue.Day.Value);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL2PartialUncertainApprox3() {
			const string DateString = "2004-(06)?-11";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(2004, TestDate.StartValue.Year.Value);
			Assert.AreEqual(false, TestDate.StartValue.Year.IsUncertain);
			Assert.AreEqual(true, TestDate.StartValue.Month.IsUncertain);
			Assert.AreEqual(false, TestDate.StartValue.Day.IsUncertain);
			Assert.AreEqual(6, TestDate.StartValue.Month.Value);
			Assert.AreEqual(11, TestDate.StartValue.Day.Value);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL2PartialUncertainApprox4() {
			const string DateString = "2004-06-(11)~";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(2004, TestDate.StartValue.Year.Value);
			Assert.AreEqual(false, TestDate.StartValue.Year.IsApproximate);
			Assert.AreEqual(false, TestDate.StartValue.Month.IsApproximate);
			Assert.AreEqual(true, TestDate.StartValue.Day.IsApproximate);
			Assert.AreEqual(6, TestDate.StartValue.Month.Value);
			Assert.AreEqual(11, TestDate.StartValue.Day.Value);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL2PartialUncertainApprox5() {
			const string DateString = "2004-(06)?~";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(2004, TestDate.StartValue.Year.Value);
			Assert.AreEqual(false, TestDate.StartValue.Year.IsApproximate);
			Assert.AreEqual(false, TestDate.StartValue.Year.IsUncertain);
			Assert.AreEqual(true, TestDate.StartValue.Month.IsApproximate);
			Assert.AreEqual(true, TestDate.StartValue.Month.IsUncertain);
			Assert.AreEqual(false, TestDate.StartValue.Day.IsApproximate);
			Assert.AreEqual(false, TestDate.StartValue.Day.IsUncertain);
			Assert.AreEqual(6, TestDate.StartValue.Month.Value);
			Assert.AreEqual(false, TestDate.StartValue.Day.HasValue);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL2PartialUncertainApprox6() {
			const string DateString = "2004-(06-11)?";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(2004, TestDate.StartValue.Year.Value);
			Assert.AreEqual(false, TestDate.StartValue.Year.IsUncertain);
			Assert.AreEqual(true, TestDate.StartValue.Month.IsUncertain);
			Assert.AreEqual(true, TestDate.StartValue.Day.IsUncertain);
			Assert.AreEqual(6, TestDate.StartValue.Month.Value);
			Assert.AreEqual(11, TestDate.StartValue.Day.Value);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL2PartialUncertainApprox7() {
			const string DateString = "2004?-06-(11)~";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(2004, TestDate.StartValue.Year.Value);
			Assert.AreEqual(true, TestDate.StartValue.Year.IsUncertain);
			Assert.AreEqual(false, TestDate.StartValue.Month.IsUncertain);
			Assert.AreEqual(false, TestDate.StartValue.Day.IsUncertain);
			Assert.AreEqual(false, TestDate.StartValue.Year.IsApproximate);
			Assert.AreEqual(false, TestDate.StartValue.Month.IsApproximate);
			Assert.AreEqual(true, TestDate.StartValue.Day.IsApproximate);
			Assert.AreEqual(6, TestDate.StartValue.Month.Value);
			Assert.AreEqual(11, TestDate.StartValue.Day.Value);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL2PartialUncertainApprox8() {
			const string DateString = "(2004-(06)~)?";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(2004, TestDate.StartValue.Year.Value);
			Assert.AreEqual(true, TestDate.StartValue.Year.IsUncertain);
			Assert.AreEqual(true, TestDate.StartValue.Month.IsUncertain);
			Assert.AreEqual(false, TestDate.StartValue.Day.IsUncertain);
			Assert.AreEqual(false, TestDate.StartValue.Year.IsApproximate);
			Assert.AreEqual(true, TestDate.StartValue.Month.IsApproximate);
			Assert.AreEqual(false, TestDate.StartValue.Day.IsApproximate);
			Assert.AreEqual(6, TestDate.StartValue.Month.Value);
			Assert.AreEqual(false, TestDate.StartValue.Day.HasValue);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL2PartialUncertainApprox9() {
			const string DateString = "2004?-(06)?~";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(2004, TestDate.StartValue.Year.Value);
			Assert.AreEqual(true, TestDate.StartValue.Year.IsUncertain);
			Assert.AreEqual(true, TestDate.StartValue.Month.IsUncertain);
			Assert.AreEqual(false, TestDate.StartValue.Day.IsUncertain);
			Assert.AreEqual(false, TestDate.StartValue.Year.IsApproximate);
			Assert.AreEqual(true, TestDate.StartValue.Month.IsApproximate);
			Assert.AreEqual(false, TestDate.StartValue.Day.IsApproximate);
			Assert.AreEqual(6, TestDate.StartValue.Month.Value);
			Assert.AreEqual(false, TestDate.StartValue.Day.HasValue);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL2PartialUncertainApprox10() {
			const string DateString = "(2004)?-06-04~";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(2004, TestDate.StartValue.Year.Value);
			Assert.AreEqual(true, TestDate.StartValue.Year.IsUncertain);
			Assert.AreEqual(false, TestDate.StartValue.Month.IsUncertain);
			Assert.AreEqual(false, TestDate.StartValue.Day.IsUncertain);
			Assert.AreEqual(false, TestDate.StartValue.Year.IsApproximate);
			Assert.AreEqual(true, TestDate.StartValue.Month.IsApproximate);
			Assert.AreEqual(true, TestDate.StartValue.Day.IsApproximate);
			Assert.AreEqual(6, TestDate.StartValue.Month.Value);
			Assert.AreEqual(4, TestDate.StartValue.Day.Value);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL2PartialUncertainApprox11() {
			const string DateString = "(2011)-06-04~";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(2011, TestDate.StartValue.Year.Value);
			Assert.AreEqual(false, TestDate.StartValue.Year.IsApproximate);
			Assert.AreEqual(true, TestDate.StartValue.Month.IsApproximate);
			Assert.AreEqual(true, TestDate.StartValue.Day.IsApproximate);
			Assert.AreEqual(6, TestDate.StartValue.Month.Value);
			Assert.AreEqual(4, TestDate.StartValue.Day.Value);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL2PartialUncertainApprox12() {
			const string DateString = "2011-(06-04)~";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(2011, TestDate.StartValue.Year.Value);
			Assert.AreEqual(false, TestDate.StartValue.Year.IsApproximate);
			Assert.AreEqual(true, TestDate.StartValue.Month.IsApproximate);
			Assert.AreEqual(true, TestDate.StartValue.Day.IsApproximate);
			Assert.AreEqual(6, TestDate.StartValue.Month.Value);
			Assert.AreEqual(4, TestDate.StartValue.Day.Value);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL2PartialUncertainApprox13() {
			const string DateString = "2011-23~";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(2011, TestDate.StartValue.Year.Value);
			Assert.AreEqual(true, TestDate.StartValue.Month.IsApproximate);
			Assert.AreEqual(true, TestDate.StartValue.Year.IsApproximate);
			Assert.AreEqual(false, TestDate.StartValue.Day.IsApproximate);
			Assert.AreEqual(Seasons.Autumn, TestDate.StartValue.Month.Value);
			Assert.AreEqual(false, TestDate.StartValue.Day.HasValue);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		// TODO
	}

	[TestFixture()] public class TestL2PartialUnspecified {

		[Test] public void TestL2PartialUnspecified1() {
			const string DateString = "156u-12-25";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(1560, TestDate.StartValue.Year.Value);
			Assert.AreEqual(12, TestDate.StartValue.Month.Value);
			Assert.AreEqual(25, TestDate.StartValue.Day.Value);
			Assert.AreEqual(0001, TestDate.StartValue.Year.UnspecifiedMask);
			Assert.AreEqual(0, TestDate.StartValue.Month.UnspecifiedMask);
			Assert.AreEqual(0, TestDate.StartValue.Day.UnspecifiedMask);
			Assert.AreEqual(true, TestDate.StartValue.Year.HasValue);
			Assert.AreEqual(true, TestDate.StartValue.Month.HasValue);
			Assert.AreEqual(true, TestDate.StartValue.Day.HasValue);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL2PartialUnspecified2() {
			const string DateString = "15uu-12-25";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(1500, TestDate.StartValue.Year.Value);
			Assert.AreEqual(12, TestDate.StartValue.Month.Value);
			Assert.AreEqual(25, TestDate.StartValue.Day.Value);
			Assert.AreEqual(0011, TestDate.StartValue.Year.UnspecifiedMask);
			Assert.AreEqual(0, TestDate.StartValue.Month.UnspecifiedMask);
			Assert.AreEqual(0, TestDate.StartValue.Day.UnspecifiedMask);
			Assert.AreEqual(true, TestDate.StartValue.Year.HasValue);
			Assert.AreEqual(true, TestDate.StartValue.Month.HasValue);
			Assert.AreEqual(true, TestDate.StartValue.Day.HasValue);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL2PartialUnspecified3() {
			const string DateString = "15uu-12-uu";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(1500, TestDate.StartValue.Year.Value);
			Assert.AreEqual(12, TestDate.StartValue.Month.Value);
			Assert.AreEqual(0, TestDate.StartValue.Day.Value);
			Assert.AreEqual(0011, TestDate.StartValue.Year.UnspecifiedMask);
			Assert.AreEqual(0, TestDate.StartValue.Month.UnspecifiedMask);
			Assert.AreEqual(11, TestDate.StartValue.Day.UnspecifiedMask);
			Assert.AreEqual(true, TestDate.StartValue.Year.HasValue);
			Assert.AreEqual(true, TestDate.StartValue.Month.HasValue);
			Assert.AreEqual(true, TestDate.StartValue.Day.HasValue);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL2PartialUnspecified4() {
			const string DateString = "1560-uu-25";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(1560, TestDate.StartValue.Year.Value);
			Assert.AreEqual(0, TestDate.StartValue.Month.Value);
			Assert.AreEqual(25, TestDate.StartValue.Day.Value);
			Assert.AreEqual(0000, TestDate.StartValue.Year.UnspecifiedMask);
			Assert.AreEqual(11, TestDate.StartValue.Month.UnspecifiedMask);
			Assert.AreEqual(00, TestDate.StartValue.Day.UnspecifiedMask);
			Assert.AreEqual(true, TestDate.StartValue.Year.HasValue);
			Assert.AreEqual(true, TestDate.StartValue.Month.HasValue);
			Assert.AreEqual(true, TestDate.StartValue.Day.HasValue);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

	}

	[TestFixture()] public class TestL2OneOfASet {
		// TODO
	}

	[TestFixture()] public class TestL2MultipleDates {
		// TODO
	}

	[TestFixture()] public class TestL2MaskedPrecision {

		[Test] public void TestL2MaskedPrecision1() {
			const string DateString = "196x";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(1960, TestDate.StartValue.Year.Value);
			Assert.AreEqual(0, TestDate.StartValue.Month.Value);
			Assert.AreEqual(0, TestDate.StartValue.Day.Value);
			Assert.AreEqual(0000, TestDate.StartValue.Year.UnspecifiedMask);
			Assert.AreEqual(1, TestDate.StartValue.Year.InsignificantDigits);
			Assert.AreEqual(true, TestDate.StartValue.Year.HasValue);
			Assert.AreEqual(false, TestDate.StartValue.Month.HasValue);
			Assert.AreEqual(false, TestDate.StartValue.Day.HasValue);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL2MaskedPrecision2() {
			const string DateString = "19xx";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(1900, TestDate.StartValue.Year.Value);
			Assert.AreEqual(0, TestDate.StartValue.Month.Value);
			Assert.AreEqual(0, TestDate.StartValue.Day.Value);
			Assert.AreEqual(0000, TestDate.StartValue.Year.UnspecifiedMask);
			Assert.AreEqual(2, TestDate.StartValue.Year.InsignificantDigits);
			Assert.AreEqual(true, TestDate.StartValue.Year.HasValue);
			Assert.AreEqual(false, TestDate.StartValue.Month.HasValue);
			Assert.AreEqual(false, TestDate.StartValue.Day.HasValue);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			Assert.AreEqual(DateString, TestDate.ToString());
		}

	}

	[TestFixture()] public class TestL2ExtendedInterval {
		// TODO
	}

	[TestFixture()] public class TestL2LongYearExponential {

		[Test] public void TestL2LongYearExponential1() {
			const string DateString = "y17e7";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(170000000, TestDate.StartValue.Year.Value);
			Assert.AreEqual(0, TestDate.StartValue.Month.Value);
			Assert.AreEqual(0, TestDate.StartValue.Day.Value);
			Assert.AreEqual(0, TestDate.StartValue.Year.UnspecifiedMask);
			Assert.AreEqual(0, TestDate.StartValue.Year.InsignificantDigits);
			Assert.AreEqual(true, TestDate.StartValue.Year.HasValue);
			Assert.AreEqual(false, TestDate.StartValue.Month.HasValue);
			Assert.AreEqual(false, TestDate.StartValue.Day.HasValue);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			//Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL2LongYearExponential2() {
			const string DateString = "y-17e7";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(-170000000, TestDate.StartValue.Year.Value);
			Assert.AreEqual(0, TestDate.StartValue.Month.Value);
			Assert.AreEqual(0, TestDate.StartValue.Day.Value);
			Assert.AreEqual(0, TestDate.StartValue.Year.UnspecifiedMask);
			Assert.AreEqual(0, TestDate.StartValue.Year.InsignificantDigits);
			Assert.AreEqual(true, TestDate.StartValue.Year.HasValue);
			Assert.AreEqual(false, TestDate.StartValue.Month.HasValue);
			Assert.AreEqual(false, TestDate.StartValue.Day.HasValue);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			//Assert.AreEqual(DateString, TestDate.ToString());
		}

		[Test] public void TestL2LongYearExponential3() {
			const string DateString = "y17101e4p3";
			var TestDate = Edtf.DatePair.Parse(DateString);
			Assert.AreEqual(171010000, TestDate.StartValue.Year.Value);
			Assert.AreEqual(0, TestDate.StartValue.Month.Value);
			Assert.AreEqual(0, TestDate.StartValue.Day.Value);
			Assert.AreEqual(0, TestDate.StartValue.Year.UnspecifiedMask);
			Assert.AreEqual(6, TestDate.StartValue.Year.InsignificantDigits);
			Assert.AreEqual(true, TestDate.StartValue.Year.HasValue);
			Assert.AreEqual(false, TestDate.StartValue.Month.HasValue);
			Assert.AreEqual(false, TestDate.StartValue.Day.HasValue);
			Assert.AreEqual(DateStatus.Normal, TestDate.StartValue.Status);
			Assert.AreEqual(DateStatus.Unused, TestDate.EndValue.Status);
			//Assert.AreEqual(DateString, TestDate.ToString());
		}

	}

	#endregion


}