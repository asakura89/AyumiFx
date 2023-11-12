Keywielder
==========

Simple token, unique key generator. It's one of my favorite library to generate formatted primary key.
Such as Document number in (LOB) apps.

# How to - 1

```csharp
String key = Wielder
    .New()
    .AddString("KEY")
    .AddString("-")
    .AddLongYear()
    .AddNumericMonth()
    .AddDate()
    .AddString("-")
    .AddLeftPadded(w => w.AddCounter(12, 10), 4, '0')
    .BuildKey()
    .Dump();

Console.WriteLine(key);
```

Above code will generate key like this (below), result may vary as I use date
*KEY-20150125-0022*

  
# How to - 2

```csharp
String complexKey = Wielder
    .New()
    .AddString("SIMPLE")
    .AddString("-")
    .AddGuidString()
    .AddString("-")
    .AddLongYear()
    .AddString("-")
    .AddRandomAlphaNumeric(10)
    .AddString("-")
    .AddRandomString(5)
    .AddString("-")
    .AddNumericDay()
    .BuildKey()
    .Dump();

Console.WriteLine(complexKey);
```

Above code will generate key like this (below), result may vary as I use year and combination random and GUID
*SIMPL-3d5968351a0645dda7bfd3bbeb4ad972-2015-CRFWSJWZ3B-SPRHG-01*
