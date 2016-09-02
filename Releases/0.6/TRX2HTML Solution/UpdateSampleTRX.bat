rd /S /Q TestResults
msbuild SampleReport.csproj
mstest /testcontainer:SampleReport\bin\debug\SampleReport.dll 
copy TestResults\*.trx SampleReport