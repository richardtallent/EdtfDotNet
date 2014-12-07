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
			Assert.AreEqual(true, TestDate.StartValue.Year.IsUncertain);
			Assert.AreEqual(true, TestDate.StartValue.Month.IsUncertain);
			Assert.AreEqual(true, TestDate.StartValue.Day.IsUncertain);
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
		// TODO
	}

	[TestFixture()] public class TestL1ExtendedInterval {
		// TODO
	}
		
	[TestFixture()] public class TestL1LongYear {
		// TODO
	}

	[TestFixture()] public class TestL1Season {
		// TODO
	}

	#endregion

	#region Level 2 Extensions

	[TestFixture()] public class TestL2PartialUncertainApprox {
		// TODO
	}

	[TestFixture()] public class TestL2PartialUnspecified {
		// TODO
	}

	[TestFixture()] public class TestL2OneOfASet {
		// TODO
	}

	[TestFixture()] public class TestL2MultipleDates {
		// TODO
	}

	[TestFixture()] public class TestL2MaskedPrecision {
		// TODO
	}

	[TestFixture()] public class TestL2ExtendedInterval {
		// TODO
	}

	[TestFixture()] public class TestL2LongYearExponential {
		// TODO
	}

	#endregion


}