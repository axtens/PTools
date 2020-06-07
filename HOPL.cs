using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Linq;

namespace PTools
{
    [Guid("9731784A-C234-4DD8-9C30-41534983FE57")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class HOPL : IHOPL
    {
		private bool debugging { get; set; }
		bool IHOPL.Debug(bool flag)
        {
			bool prev = debugging;
			debugging = flag;
			return prev;
        }
		string IHOPL.VarsToJson(string caret_terminated_bracketed_name_and_value)
        {
			if (debugging) Debugger.Launch();
            var dict = ParseVarsToDictionary(caret_terminated_bracketed_name_and_value);
			return JsonConvert.SerializeObject(new SuccessDictionaryBlock()
			{
				Error = null,
				Cargo = dict
			});
		}
        string IHOPL.RstToJson(string rst_tabs_crlf)
        {
			if (debugging) Debugger.Launch();
			var dataTable = ParseIncomingRstToArrayOfDictionary(rst_tabs_crlf);
            return JsonConvert.SerializeObject(new SuccessListDictionaryBlock
            {
                Cargo = dataTable,
                Error = null
            });
        }
        string IHOPL.SayYear(string rst_tabs_crlf, string which, string vars)
        {
			if (debugging) Debugger.Launch();
			// make a dictionary from vars assuming '[var]val^[var]val^' (caret as terminator)
			// split rst_tabs_crlf on crlf to get rows
			// then on tabs to get fields
			// then use row 0 to give headings for dictionary of array of string
			var dataTable = ParseIncomingRstToArrayOfDictionary(rst_tabs_crlf);
			Dictionary<string,string> varDict = ParseVarsToDictionary(vars);
            return string.Empty;
        }

        private Dictionary<string, string> ParseVarsToDictionary(string vars)
        {
			if (debugging) Debugger.Launch();
            var result = new Dictionary<string, string>();
			var pattern = new Regex(@"\[(\w+)\]([^^]*)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
			var matches = pattern.Matches(vars);
			foreach (Match match in matches)
            {
				result[match.Groups[1].Value] = match.Groups[2].Value;
            }
			return result;
        }

		private List<Dictionary<string, string>> ParseIncomingRstToArrayOfDictionary(string rst_tabs_crlf)
        {
			if (debugging) Debugger.Launch();
			var lineArray = rst_tabs_crlf.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            var fieldNamesArray = lineArray[0].Split(new string[] { "\t" }, StringSplitOptions.None);
            var resultArray = new List<Dictionary<string, string>>();
            for (var i = 1; i < lineArray.Length; i++)
            {
                var line = lineArray[i];
                var lineItems = line.Split(new string[] { "\t" }, StringSplitOptions.None);
                var lineData = new Dictionary<string, string>();
                for (var f = 0; f < lineItems.Length; f++)
                {
                    lineData[fieldNamesArray[f]] = lineItems[f];
                }
                resultArray.Add(lineData);

            }
            return resultArray;
        }

        /* <@ DEFUDRLIT>__SayYear|
<@ LETLSTLIT>__UserDefinedUsages|__SayYear</@>
<TD>  &nbsp;
<@ UNLETYFLDLIT>FirstYear|
<@ UNLZROFLDLIT>FirstYear|
    <!-- listLanguages.pre #1 (BMA) which == <@ SAYVAR>which</@>, CountryLabel == <@ SAYFLD>CountryLabel</@> -->
    <@ OMT>
        <@ CMPVARLIT>which|
            <@ CVL>BySCat|
                <A  title="<@ SAYVAR>zi</@><@ SAYFLD>SCDisplay</@> for <@ SAYFLD>firstYear</@>" class="ZoomText" HREF="findlanguages.prx?which=bySCatAndYear&SCat=<@ SAYFLD>SCCode</@>&Year=<@ SAYFLD>firstYear</@>">&darr;</A>
            </@>
            <@ CVL>byCountry|
                <A  title="<@ SAYVAR>zi</@><@ SAYFLD>CountryLabel</@> for <@ SAYFLD>firstYear</@>" class="ZoomText" HREF="findlanguages.prx?which=byCountryAndYear&id=<@ SAYFLD>countrycode</@>&Year=<@ SAYFLD>firstYear</@>">&darr;</A>
            </@>
            <@ CVL>ByMyCat|
                <A  title="<@ SAYVAR>zi</@><@ SAYFLD>NodeText</@> for <@ SAYFLD>firstYear</@>" class="ZoomText" HREF="findlanguages.prx?which=byMyCatAndYear&NodeID=<@ SAYFLD>NodeID</@>&Year=<@ SAYFLD>firstYear</@>">&darr;</A>
            </@>
            <@ CVL>ByYear|

            </@>
            <@ CVL>byCountryAndYear|
                <A  title="<@ SAYVAR>zo</@><@ SAYFLD>firstYear</@>" class="ZoomText" HREF="findlanguages.prx?which=ByYear&Year=<@ SAYFLD>firstYear</@>">&uarr;</A>
            </@>
            <@ CVL>bySCatAndYear|
                <A  title="<@ SAYVAR>zo</@><@ SAYFLD>firstYear</@>" class="ZoomText" HREF="findlanguages.prx?which=ByYear&Year=<@ SAYFLD>firstYear</@>">&uarr;</A>
            </@>
        </@>
    </@>

    <@ IFFVARVAR>which|BySCat|
        <A  title="<@ SAYVAR>zi</@><@ SAYFLD>SCDisplay</@> for <@ SAYFLD>firstYear</@>" class="ZoomText" HREF="findlanguages.prx?which=bySCatAndYear&SCat=<@ SAYFLD>SCCode</@>&Year=<@ SAYFLD>firstYear</@>">&darr;</A>
    </@>
    <@ IFFVARVAR>which|byCountry|
        <A  title="<@ SAYVAR>zi</@><@ SAYFLD>CountryLabel</@> for <@ SAYFLD>firstYear</@>" class="ZoomText" HREF="findlanguages.prx?which=byCountryAndYear&id=<@ SAYFLD>countrycode</@>&Year=<@ SAYFLD>firstYear</@>">&darr;</A>
    </@>
    <@ IFFVARVAR>which|ByMyCat|
        <A  title="<@ SAYVAR>zi</@><@ SAYFLD>NodeText</@> for <@ SAYFLD>firstYear</@>" class="ZoomText" HREF="findlanguages.prx?which=byMyCatAndYear&NodeID=<@ SAYFLD>NodeID</@>&Year=<@ SAYFLD>firstYear</@>">&darr;</A>
    </@>
    <@ IFFVARVAR>which|ByYear|

    </@>
    <@ IFFVARVAR>which|byCountryAndYear|
        <A  title="<@ SAYVAR>zo</@><@ SAYFLD>firstYear</@>" class="ZoomText" HREF="findlanguages.prx?which=ByYear&Year=<@ SAYFLD>firstYear</@>">&uarr;</A>
    </@>
    <@ IFFVARVAR>which|bySCatAndYear|
        <A  title="<@ SAYVAR>zo</@><@ SAYFLD>firstYear</@>" class="ZoomText" HREF="findlanguages.prx?which=ByYear&Year=<@ SAYFLD>firstYear</@>">&uarr;</A>
    </@>

    <@ UNLVARLITLIT>which|ByYear|
        <A  class="FlipText" HREF="findlanguages.prx?year=<@ SAYFLD>firstYear</@>&which=byyear">&oplus;</A>
    </@>
    <@ SAYFLD>FirstYear</@>
</@>
</@>
</TD>
</@>
*/
        string IHOPL.SaySammet(string rst_tabs_crlf, string which, string vars)
        {
			if (debugging) Debugger.Launch();
			return string.Empty;
        }
        string IHOPL.SayNode(string rst_tabs_crlf, string which, string vars)
        {
			if (debugging) Debugger.Launch();
			return string.Empty;
        }
        string IHOPL.SayCountry(string rst_tabs_crlf, string which, string vars)
        {
			if (debugging) Debugger.Launch();
			return string.Empty;
        }

    }
}

/*


<@ DEFUDRLIT>__SaySammet|
	<@ LETLSTLIT>__UserDefinedUsages|__SaySammet</@>
	<TD>  &nbsp;
	<@ UNLETYFLDLIT>SCCode|
<!-- listLanguages.pre #2 (BMA) which == <@ SAYVAR>which</@> -->
		<@ OMT>
			<@ CMPVARLIT>which|
				<@ CVL>BySCat|

				</@>
				<@ CVL>byCountry|

				</@>
				<@ CVL>ByMyCat|

				</@>
				<@ CVL>ByYear|
					<A  title="<@ SAYVAR>zi</@><@ SAYFLD>SCDisplay</@> for <@ SAYFLD>firstYear</@>" class="ZoomText" HREF="findlanguages.prx?which=bySCatAndYear&SammetCat=<@ SAYFLD>SCCode</@>&Year=<@ SAYFLD>firstYear</@>">&darr;</A>
				</@>
				<@ CVL>bySCatAndYear|
					<A  title="<@ SAYVAR>zo</@><@ SAYFLD>SCDisplay</@>" class="ZoomText" HREF="findlanguages.prx?which=BySCat&SammetCat=<@ SAYFLD>SCCode</@>">&uarr;</A>
				</@>
			</@>
		</@>
		<@ IFFVARVAR>BySCat|

		</@>
		<@ IFFVARVAR>byCountry|

		</@>
		<@ IFFVARVAR>ByMyCat|

		</@>
		<@ IFFVARVAR>ByYear|
			<A  title="<@ SAYVAR>zi</@><@ SAYFLD>SCDisplay</@> for <@ SAYFLD>firstYear</@>" class="ZoomText" HREF="findlanguages.prx?which=bySCatAndYear&SammetCat=<@ SAYFLD>SCCode</@>&Year=<@ SAYFLD>firstYear</@>">&darr;</A>
		</@>
		<@ IFFVARVAR>bySCatAndYear|
			<A  title="<@ SAYVAR>zo</@><@ SAYFLD>SCDisplay</@>" class="ZoomText" HREF="findlanguages.prx?which=BySCat&SammetCat=<@ SAYFLD>SCCode</@>">&uarr;</A>
		</@>

		<@ UNLVARLITLIT>which|BySCat|
			<A  class="FlipText" HREF="findlanguages.prx?which=BySCat&SammetCat=<@ SAYFLD>SCCode</@>">&oplus;</A>
		</@>
		<@ SAYFLD>SCDisplay</@>
	</@>
	</TD>



</@>

<@ DEFUDRLIT>__SayNode|
	<@ LETLSTLIT>__UserDefinedUsages|__SayNode</@>
	<TD>
	<@ UNLETYFLDLIT>NodeText|
<!-- listLanguages.pre #3 (BMA) which == <@ SAYVAR>which</@> -->
		<@ OMT>
			<@ CMPVARLIT>which|
				<@ CVL>ByMyCat|

				</@>
				<@ CVL>ByYear|
					<A  title="<@ SAYVAR>zi</@><@ SAYFLD>NodeText</@> for <@ SAYFLD>firstYear</@>" class="ZoomText" HREF="findlanguages.prx?which=byMyCatAndYear&NodeID=<@ SAYFLD>NodeID</@>&Year=<@ SAYFLD>firstYear</@>">&darr;</A>
				</@>
				<@ CVL>byMyCatAndYear|
					<A  title="<@ SAYVAR>zo</@><@ SAYFLD>NodeText</@>" class="ZoomText" HREF="findlanguages.prx?which=ByMyCat&NodeID=<@ SAYFLD>NodeID</@>">&uarr;</A>
				</@>
				<@ CVL>byMyCatAndCountry|
					<A  title="<@ SAYVAR>zo</@><@ SAYFLD>NodeText</@>" class="ZoomText" HREF="findlanguages.prx?which=ByMyCat&NodeID=<@ SAYFLD>NodeID</@>">&uarr;</A>
				</@>
			</@>
		</@>

		<@ IFFVARVAR>which|ByMyCat|

		</@>
		<@ IFFVARVAR>which|>ByYear|
			<A  title="<@ SAYVAR>zi</@><@ SAYFLD>NodeText</@> for <@ SAYFLD>firstYear</@>" class="ZoomText" HREF="findlanguages.prx?which=byMyCatAndYear&NodeID=<@ SAYFLD>NodeID</@>&Year=<@ SAYFLD>firstYear</@>">&darr;</A>
		</@>
		<@ IFFVARVAR>which|byMyCatAndYear|
			<A  title="<@ SAYVAR>zo</@><@ SAYFLD>NodeText</@>" class="ZoomText" HREF="findlanguages.prx?which=ByMyCat&NodeID=<@ SAYFLD>NodeID</@>">&uarr;</A>
		</@>
		<@ IFFVARVAR>which|byMyCatAndCountry|
			<A  title="<@ SAYVAR>zo</@><@ SAYFLD>NodeText</@>" class="ZoomText" HREF="findlanguages.prx?which=ByMyCat&NodeID=<@ SAYFLD>NodeID</@>">&uarr;</A>
		</@>

		<@ UNLVARLITLIT>which|ByMyCat|
			<A  class="FlipText" title = "Flip around to <@ SAYFLD>NodeText</@> languages" HREF="findlanguages.prx?which=byMyCat&NodeID=<@ SAYFLD>NodeID</@>">&oplus;</A>
		</@>
		<@ SAYFLD>NodeText</@>
	</@>&nbsp;
	</TD>
</@>

<@ DEFUDRLIT>__SayCountry|
	<@ LETLSTLIT>__UserDefinedUsages|__SayCountry</@>
	<TD>
<!-- listLanguages.pre #4 (BMA) which == <@ SAYVAR>which</@>, CountryLabel == <@ SAYFLD>CountryLabel</@>  -->
<!-- <@ SAYPOSRST>..</@>: <@ SAYFLD>CountryLabel</@> -->
	<@ UNLETYFLDLIT>CountryLabel|
		<@ OMT>
			<@ CMPVARLIT>which|
				<@ CVL>byMyCatAndCountry|
					<A  title="<@ SAYVAR>zo</@><@ SAYFLD>CountryLabel</@>" class="ZoomText" HREF="findlanguages.prx?which=ByCountry&id=<@ SAYFLD>countrycode</@>">&uarr;</A>
				</@>
				<@ CVL>byCountryAndYear|
					<A  title="<@ SAYVAR>zo</@><@ SAYFLD>CountryLabel</@>" class="ZoomText" HREF="findlanguages.prx?which=ByCountry&id=<@ SAYFLD>countrycode</@>">&uarr;</A>
				</@>
				<@ CVL>ByMyCat|
					<A  title="<@ SAYVAR>zi</@><@ SAYFLD>CountryLabel</@> for <@ SAYFLD>NodeText</@>" class="ZoomText" HREF="findlanguages.prx?which=byMyCatAndCountry&ID=<@ SAYFLD>countrycode</@>&NodeID=<@ SAYFLD>NodeID</@>">&darr;</A>
				</@>
				<@ CVL>ByMyCatAndYear|
					<A  title="<@ SAYVAR>zi</@><@ SAYFLD>CountryLabel</@> for <@ SAYFLD>firstYear</@> and <@ SAYFLD>SCDisplay</@>" class="ZoomText" HREF="findlanguages.prx?which=bySCodeCountryAndYear&id=<@ SAYFLD>countrycode</@>&Year=<@ SAYFLD>firstYear</@>">&darr;</A>
				</@>
				<@ CVL>ByYear|
					<A  title="<@ SAYVAR>zi</@><@ SAYFLD>CountryLabel</@> for <@ SAYFLD>firstYear</@>" class="ZoomText" HREF="findlanguages.prx?which=byCountryAndYear&id=<@ SAYFLD>countrycode</@>&Year=<@ SAYFLD>firstYear</@>">&darr;</A>
				</@>
			</@>
		</@>
				<@ IFFVARVAR>which|byMyCatAndCountry|
					<A  title="<@ SAYVAR>zo</@><@ SAYFLD>CountryLabel</@>" class="ZoomText" HREF="findlanguages.prx?which=ByCountry&id=<@ SAYFLD>countrycode</@>">&uarr;</A>
				</@>
				<@ IFFVARVAR>which|byCountryAndYear|
					<A  title="<@ SAYVAR>zo</@><@ SAYFLD>CountryLabel</@>" class="ZoomText" HREF="findlanguages.prx?which=ByCountry&id=<@ SAYFLD>countrycode</@>">&uarr;</A>
				</@>
				<@ IFFVARVAR>which|ByMyCat|
					<A  title="<@ SAYVAR>zi</@><@ SAYFLD>CountryLabel</@> for <@ SAYFLD>NodeText</@>" class="ZoomText" HREF="findlanguages.prx?which=byMyCatAndCountry&ID=<@ SAYFLD>countrycode</@>&NodeID=<@ SAYFLD>NodeID</@>">&darr;</A>
				</@>
				<@ IFFVARVAR>which|ByMyCatAndYear|
					<A  title="<@ SAYVAR>zi</@><@ SAYFLD>CountryLabel</@> for <@ SAYFLD>firstYear</@> and <@ SAYFLD>SCDisplay</@>" class="ZoomText" HREF="findlanguages.prx?which=bySCodeCountryAndYear&id=<@ SAYFLD>countrycode</@>&Year=<@ SAYFLD>firstYear</@>">&darr;</A>
				</@>
				<@ IFFVARVAR>which|ByYear|
					<A  title="<@ SAYVAR>zi</@><@ SAYFLD>CountryLabel</@> for <@ SAYFLD>firstYear</@>" class="ZoomText" HREF="findlanguages.prx?which=byCountryAndYear&id=<@ SAYFLD>countrycode</@>&Year=<@ SAYFLD>firstYear</@>">&darr;</A>
				</@>

		<@ UNLVARLITLIT>which|ByCountry|
			<A  class="FlipText" HREF="findlanguages.prx?which=byCountry&id=<@ SAYFLD>countrycode</@>">&oplus;</A>
		</@>
		<@ SAYFLD>CountryLabel</@>
	</@>&nbsp;
	</TD>
</@>
*/


