msbuild /t:clean;rebuild /p:Configuration=Release "TRX2HTML Solution.sln"
md Releases
cd Releases
md 0.5.200
cd 0.5.200
copy ..\..\trx2html\bin\release\trx2html.exe .
copy ..\..\RidoTasks.trx2html\bin\release\RidoTasks.dll .
cd ..