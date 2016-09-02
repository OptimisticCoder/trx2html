msbuild /t:clean;rebuild /p:Configuration=Release "TRX2HTML Solution.sln"
md Releases
cd Releases
md 0.6
cd 0.6
copy ..\..\trx2html\bin\release\trx2html.exe .
copy ..\..\RidoTasks.trx2html\bin\release\RidoTasks.dll .
copy ..\..\RidoTasks.targets .
copy ..\..\CHANGELOG.txt .
cd ..
start .
cd ..