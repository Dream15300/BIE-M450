using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace BIE.E2ETests;

[TestClass]
public class LearnerE2ETests : BrowserTestBase
{
    // Anzeigen, wie er den Test in Chrome am ausführen ist:
    // Für diesen Test im Terminal in PS C:\_GBS_ZbW\GBS\M450_Applikationen_testen\Starter\Starter> folgendes einfügen:
    // $env:SHOW_BROWSER = "1"
    // dotnet test tests / BIE.E2ETests
        [TestMethod]
    public void TouristAt30DaysRemainsApprovedAfterReload()
    {
        Fill("Sam Browser", "Tourismus", "30");
        // TODO: Absenden, sichtbares Ergebnis abwarten und prüfen.

        /*
            Driver	                Steuert den Browser über Selenium.
            FindElement()	        Sucht ein HTML-Element.
            By.Id("submit")	        Sucht das Element mit der ID submit.
            .Click()	            Simuliert einen Mausklick.
            Wait.Until(...)	        Wartet, bis die angegebene Bedingung erfüllt ist.
            .Displayed	            Prüft, ob das Element sichtbar ist.
            .Text	                Liest den sichtbaren Text eines Elements.
            StringAssert.Contains()	Prüft, ob der Text GENEHMIGT enthält. 
        */

        // Act
        Driver.FindElement(By.Id("submit")).Click();

        // Assert
        Wait.Until(d => d.FindElement(By.Id("result")).Displayed);
        StringAssert.Contains(
            Driver.FindElement(By.Id("result")).Text,
            "GENEHMIGT"
        );

        var caseId = Driver.FindElement(By.Id("result")).GetDomAttribute("data-application-id");
        Driver.Navigate().Refresh();
        var row = Wait.Until(d => d.FindElements(By.CssSelector("#application-list tr")).FirstOrDefault(r => r.GetDomAttribute("data-id") == caseId));
        var cells = row.FindElements(By.TagName("td"));
        // TODO: Name, Herkunft, Tage und Entscheidung nach Neuladen prüfen.
        // Assert
        Assert.IsNotNull(row);                              // Der gespeicherte Antrag wurde gefunden
        Assert.AreEqual("Sam Browser", cells[1].Text);      // Name ist Sam Browser
        Assert.AreEqual("Orion", cells[2].Text);            // Herkunft ist Orion
        Assert.AreEqual("30 Tage", cells[3].Text);          // Aufenthaltsdauer ist 30 Tage
        StringAssert.Contains(cells[4].Text, "GENEHMIGT");  // Entscheidung enthält GENEHMIGT
    }
}
