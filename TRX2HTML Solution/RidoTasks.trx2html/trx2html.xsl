<?xml version='1.0' encoding='utf-8' ?>
<!--
	rido'2006: http://blogs.msdn.com/rido
-->
<xsl:stylesheet version="1.0" 
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:msxsl="urn:schemas-microsoft-com:xslt" 
	xmlns:trxreport="urn:my-scripts">
	
	<xsl:output method="html"/>
	<xsl:key name="TestClasses" match="testMethod" use="className"/>

	<msxsl:script language="C#" implements-prefix="trxreport">
		public string RemoveAssemblyName(string asm) {
			return asm.Substring(0,asm.IndexOf(','));
		}
		public string RemoveNamespace(string asm) {
			int coma = asm.IndexOf(',');
			return asm.Substring(coma + 2, asm.Length - coma - 2);
		}
	</msxsl:script>
	
	<xsl:template match="/">
		<html>
			<head>
				<link type="text/css" rel="StyleSheet"  href="trx2html.css"   />
				<script language="javascript" type="text/javascript" src="trx2html.js"></script>
				<title>
					TRX Report -
					<xsl:value-of select="trxreport:RemoveNamespace(/Tests/TestRun/tests/value/testMethod/className)"/>
				</title>
			</head>
			<body onload="summary();">
				<a name="__top" />
				<h3>TRX Report - 
					<xsl:value-of select="trxreport:RemoveNamespace(/Tests/TestRun/tests/value/testMethod/className)"/>
				</h3>

				<div class="contents">
					<a href="#totals">Totals</a>
					|
					<a href="#summary">Summary</a>
					|
					<a href="#detail">Detail</a>
					|
					<a href="#envInfo">Environment Information</a>				
				</div>
				<br />
				<a name="totals" />
				<table id="tMainSummary"  border="0">
					<tr>
						<th>Percent</th>
						<th>Status</th>
						<th>TotalTests</th>
						<th>Passed</th>
						<th>Failed</th>
						<th>Ignored</th>
						<th>TimeTaken</th>
					</tr>
					<tr>
						<td></td>
						<td width="350px" style="vertical-align:middle;font-size:200%"></td>
						<td>
							<xsl:value-of select="/Tests/TestRun/result/totalTestCount"/>
						</td>
						<td>
							<xsl:value-of select="/Tests/TestRun/result/passedTestCount"/>
						</td>
						
						<td></td>
						
						<td></td>
						
						<td></td>
					</tr>
				</table>
				<br />

				<a name="summary" />				
				<table id="tSummaryDetail"  border="0">
					<tr>
						<th>TestClasses Summary</th>
						<th>Percent</th>
						<th>Status</th>
						<th>TestsPassed</th>
						<th>TestsFailed</th>
						<th>TestsIgnored</th>
						<th>Duration</th>
					</tr>
					<xsl:for-each select="//testMethod[generate-id(.)=generate-id(key('TestClasses', className))]">
						<tr>
							<td>
								<a>
									<xsl:attribute name="href">
										<xsl:value-of select="'#'"/>
										<xsl:value-of select="generate-id(className)"/>
									</xsl:attribute>
									<xsl:value-of select="trxreport:RemoveAssemblyName(className)" />
								</a>
							</td>

							<!-- Percent -->
							<td></td>
							
							<!-- status -->
							<td width="80px"></td>
							
							<!-- success -->
							<td></td>
							
							<!-- failed-->
							<td></td>

							<!-- ignored-->
							<td></td>


							<!-- Duration -->
							<td></td>
						</tr>
					</xsl:for-each>
				</table>

				<br />
				<a name="detail" />
				<i>Test Class Detail</i>
			
				<xsl:for-each select="//testMethod[generate-id(.)=generate-id(key('TestClasses', className))]">
					<h5>
						
					</h5>

					<a name="{generate-id(className)}" />
					<table border="0">
						<tr>
							<th colspan="4">
								<b><xsl:value-of select="trxreport:RemoveAssemblyName(className)" /></b>		
							</th>
						</tr>
					

						<xsl:for-each select="key('TestClasses', className)" >
							<tr>
								<xsl:call-template name="tDetails">
									<xsl:with-param name="testId" select="./../id/id/text()" />
									<xsl:with-param name="testDescription" select="./../description" />
								</xsl:call-template>
							</tr>
						</xsl:for-each>
					</table>
					<a href="#__top">Back to top</a>
					<br />
				</xsl:for-each>

				<br />
				<a name="envInfo" />
				<xsl:call-template name="envInfo" />
				<hr style="border-style:dotted;color:#dcdcdc"/>
				<i style="width:100%;font:10pt Verdana;text-align:center;background-color:#dcdcdc">The VSTS Test Results HTML Viewer. (c) <a href="http://blogs.msdn.com/rido">rido</a>'06</i>
			</body>
		</html>
	</xsl:template>

	<xsl:template name="tDetails">
		<xsl:param name="testId" />
		<xsl:param name="testDescription" />
		<xsl:for-each select="/Tests/UnitTestResult[id/testId/id=$testId]">
			<td>
				<xsl:value-of select="testName"/>				
			</td>
			
				<xsl:choose>
					<xsl:when test="outcome/value__=1">
						<td>
							<p class="testKo"
							   title="Click to see the StackTrace" 
							   onmouseover="this.style.color='orange'"
								onmouseout="this.style.color='red'" 
								onclick="togle('{generate-id(.)}')">n</p>
						</td>
						<td width="300px">
							
							<xsl:value-of select="$testDescription"/>
							<br />
							<xsl:value-of select="errorInfo/message/text()"/>
						
							<div id="{generate-id(.)}" class="trace"  style="display:none">
								<xsl:call-template name="debugInfo">
									<xsl:with-param name="testId" select="$testId" />
								</xsl:call-template>
								<pre  class="failureInfo" >														
									<xsl:value-of select="errorInfo/stackTrace/text()"/>
								</pre>
							</div>
						</td>
					</xsl:when>
					<xsl:when test="outcome/value__=10">
						<td>
							<p class="testOk"
								title="Click to see Test Trace" 
							   onmouseover="this.style.color='green'"
								onmouseout="this.style.color='lime'" 
								onclick="togle('{generate-id(.)}')">n</p>
						</td>
						<td width="300px">
							
							<xsl:value-of select="$testDescription"/>
							<div id="{generate-id(.)}" class="trace" style="display:none">
								<xsl:call-template name="debugInfo">
									<xsl:with-param name="testId" select="$testId" />
								</xsl:call-template>
							</div>
							</td>						
					</xsl:when>
					<xsl:otherwise>
						<td>
							<p class="testIgnore"
								title="Click to see test Trace" 
							   onmouseover="this.style.color='white'"
								onmouseout="this.style.color='yellow'" 
								onclick="togle('{generate-id(.)}')">n</p>
						</td>
						<td width="300px">
							<xsl:value-of select="$testDescription"/>
							<br />
							<xsl:value-of select="errorInfo/message/text()"/>

							<div id="{generate-id(.)}" class="trace" style="display:none">
								<xsl:call-template name="debugInfo">
									<xsl:with-param name="testId" select="$testId" />
								</xsl:call-template>

							</div>
						</td>
					</xsl:otherwise>
				</xsl:choose>			
			<td>
			<xsl:value-of select="duration" />
			</td>
		</xsl:for-each>
	</xsl:template>

	<xsl:template name="debugInfo">
		<xsl:param name="testId" />
		<xsl:for-each select="/Tests/UnitTestResult[id/testId/id=$testId]">
			
				<div class="border">
					<xsl:value-of select="stdout"/>
					<br />				
					<xsl:value-of select="stderr"/>
					<br />
					<xsl:value-of select="debugTrace"/>
					<br />
					<xsl:value-of select="traceInfo/trace"/>
				</div>						
		</xsl:for-each>			
	</xsl:template>

	<xsl:template name="envInfo">
		<table>
			<tr>
				<th colspan="2">TestRun Environment Information</th>
			</tr>
			<tr>
				<th align="right">TestCodebase</th>
				<td>
					<xsl:value-of select="//codeBase"/>
				</td>
			</tr>
			<tr>
				<th align="right">AssemblyUnderTest</th>
				<td>
					<xsl:value-of select="//deploymentItems/m_container/key/path" />
				</td>
			</tr>
			<tr>
				<th align="right">MachineName</th>
				<td>
					<xsl:value-of select="//m_computerName"/>
				</td>
			</tr>
			<tr>
				<th align="right">UserName</th>
				<td>
					<xsl:value-of select="//runUser"/>
				</td>
			</tr>
			<tr>
				<th align="right">Original TRXFile</th>
				<td>
					<xsl:value-of select="//name"/>
				</td>
			</tr>
		</table>
		
	</xsl:template>

</xsl:stylesheet>
