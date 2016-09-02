SET nxslt="%ProgramFiles%\nxslt\nxslt2.exe"

%nxslt% %1 trx2html\trx2html.xsl -o %1.htm
