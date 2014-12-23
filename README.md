EDTF.NET
==========

EDTF.NET is a C#-based implementation of the Extended Date/Time Format (EDTF).

EDTF is a data model for dates and times that is based on a subset of ISO 8601 and is extended to cover use cases where some or all of a date is uncertain, approximate, or unknown.

If you aren't familiar with EDTF, here's more information:

http://www.loc.gov/standards/datetime/pre-submission.html

Rather than attempting to create a full parallel implementation of DateTime, EDTF.NET's primary focus is parsing strings containing EDTF data into discrete properties and serializing EDTF date structures to conforming strings.

History and Current Status
==========================

I wrote this library primarily so I could have a deep understanding myself of EDTF's data model. I don't currently have any applications under development that use it, though I have a kernel of an idea that may turn into one.

Current Status
==========================

While basic, the code can parse, store, and emit individual dates, pairs of dates (intervals/ranges), and lists of dates (actually lists of pairs of dates, both in "one of a set" and "multiple" modes). 

Unit tests have been created to cover L0, L1, and some L2 features, using the examples from the draft specification.

## To-Do (if you can help with these, that would be awesome)

DONE Resolve currently-failing L2 unit tests because of grouping/qualifier propogation issues during parsing.
- Resolve currently-failing L2 unit tests due to re-grouping issues in ToString()).
- Unit tests to cover the remaining L2 feature examples
- Unit tests to cover additional cases (such as counter-examples)
- Implicit conversion to/from DateTime, with appropriate exceptions for failures on the former
- Stronger validation to avoid illegal combinations of features (such as anything after a season in a single Date other than a qualifier).
- Utility functions for comparing individual Dates and DatePairs (e.g., "does this date fall within this datepair" or "do these datepairs overlap").
- Utility functions for comparing, merging, and collapsing DatePairLists, and comparing Dates and DatePairs to them (e.g., a "smart" version of DatePairList.Contains(Date)).
- Performance testing and tuning with large sets of data.

## Code Conventions and Decisions

- Since this is a personal project, I use Xamarin Studio, not VS.NET, so there may be occasional wrinkles in what VS expects in the solution and project files.
- This project is built to be Mono-compatible (currently v. 3.10.0).
- Structures are used for the dates and date pairs to provide immutability and to make the structures more akin to DateTime values.
- Parsing is performed using regular expressions. My first attempt used a character-based lexer, but the backtracking required for some EDTF features was a pain, and regular expressions are pretty darned fast when compiled.
- Rather than using constant escaped strings, the main regex pattern is stored as an embedded text file resource and is loaded dynamically by the library when creating the parser. The regex parser is static, compiled, and thread-safe.
- I admit my preferences for formatting C# code are a bit outside the norm. I use tabs and I tend to minimize unnecessary vertical fluff. Please don't send pull requests just to reformat my code.

Data Structure Summary
=======================

## DatePart
The main date parts (year, month, and day) have both a value and metadata regarding the presence of a value, the precision, and the certainty. This structure deals with this ambiguity the same way for all three. (EDTF time and time zone offsets don't currently support uncertain values, so time parts are just stored as integers.)

## Date
The `Date` structure is analogous to .NET's DateTime type.

## DatePair
`Date` value can appear in intervals (`d1/d2`) or ranges (`d1..d2` or `..d2` or `d1..`, but only within `[]` or `{}`). This structure supports an interval or range between a start date and end date.

## DatePairList
This IList implementation stores `DatePair` values. There are two modes for these lists: `OneOfASet` and `Multiple`. The first is the default, and indicates that the true value is one of the list of dates provided, but the actual value is unknown. This is rendered in EDTF as `[d1, d2, ...]`. The latter mode is used when _all_ of the dates in the list are equally true (for example, a list of dates where an employee received a paycheck).

### Why is there a DatePairList instead of a DateList?
EDTF allows a list (set or multiple) to contain not only single dates, but also intervals or ranges. Since the `Date` and `DatePair` types don't derive from a common base type and they don't share enough in common to merit implementing a common interface, the `IList` implementation is for `DatePair` values. Single dates can be stored in these collections easily by using a `DatePair` with the `StartValue` set to the desired date and `EndValue` unused.