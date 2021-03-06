# z010 - Yearly stock change grid 

Create a grid of yearly stock changes.  I have seen this in mutual funds but not in stocks.

Left would be the stock symbols.  Top is each year.  Each cell will have the percentage change for each year.

### Using

- [Visual Studio v16.9](https://visualstudio.microsoft.com/vs/preview)
- [.NET 5.0](https://dotnet.microsoft.com/download/dotnet/5.0)

### NuGet

- [CsvHelper](https://www.nuget.org/packages/CsvHelper)
- [MediatR.Extensions.Microsoft.DependencyInjection](https://www.nuget.org/packages/MediatR.Extensions.Microsoft.DependencyInjection)
- [Microsoft.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore)
- [Microsoft.EntityFrameworkCore.Sqlite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite)
- [Microsoft.Extensions.Http](https://www.nuget.org/packages/Microsoft.Extensions.Http)

### Roslyn Analyzers

- [StyleCop.Analyzers](https://www.nuget.org/packages/StyleCop.Analyzers)
- <s>[Microsoft.CodeAnalysis.NetAnalyzers](https://www.nuget.org/packages/Microsoft.CodeAnalysis.NetAnalyzers)</s>
  - Right click Project file > Properties > Code Analysis > "Enable .NET Analyzers" = checked.  (default is checked for C# 9.0)
- Hmm, maybe [xunit.analyzers](https://www.nuget.org/packages/xunit.analyzers)

### Unit testing

- [xUnit](https://www.nuget.org/packages/xunit)
- [xUnit.runner.visualstudio](https://www.nuget.org/packages/xunit.runner.visualstudio)
- [Moq](https://www.nuget.org/packages/Moq)
- [Microsoft.NET.Test.Sdk](Microsoft.NET.Test.Sdk) - needed for Visual Studio IDE and xUnit

### Visual Studio Settings

- Tools > Options > Text Editor > C# > Advanced > Place 'System' directives first when sorting using = Checked.

### Visual Studio Extensions

- [CodeMaid](https://marketplace.visualstudio.com/items?itemName=SteveCadwallader.CodeMaid)
- [Visual Studio Spell Checker (VS2017 and Later)](https://marketplace.visualstudio.com/items?itemName=EWoodruff.VisualStudioSpellCheckerVS2017andLater)

### NasdaqTrader refinance:
- [Symbol Lookup](http://www.nasdaqtrader.com/Trader.aspx?id=symbollookup)
- [Symbol Look-Up/Directory Data Fields & Definitions](http://www.nasdaqtrader.com/trader.aspx?id=symboldirdefs)

#### URL to download the symbols:
- http://www.nasdaqtrader.com/dynamic/SymDir/nasdaqlisted.txt
- http://www.nasdaqtrader.com/dynamic/SymDir/otherlisted.txt
