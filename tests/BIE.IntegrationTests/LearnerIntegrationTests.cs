using System.Net;
using System.Net.Http.Json;
using BIE.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bie.IntegrationTests;

[TestClass]
public class LearnerIntegrationTests
{
    [TestMethod]
    public async Task TouristAt30DaysIsSavedAsApproved()
    {
        using var app = new BieFactory();
        using var client = app.CreateClient();
        var entry = new Entry("Sam Test", "Orion", "Tourismus", 30, false, false, false, true);
        var response = await client.PostAsJsonAsync("/api/cases", entry);
        var records = await client.GetFromJsonAsync<List<Case>>("/api/cases");
        // TODO: HTTP-Status, gespeicherte Werte und Entscheidung prüfen.
        // Assert
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode); // Die API bestätigt das Erstellen mit HTTP 201
        Assert.IsNotNull(records);                                    // Die GET-Anfrage liefert eine Liste
        Assert.AreEqual(1, records.Count);                            // Genau ein Antrag ist gespeichert
        Assert.AreEqual(entry, records[0].Applicant);                 // Der gespeicherte Antrag entspricht der Eingabe
        Assert.IsTrue(records[0].Id > 0);                             // Der Antrag hat eine gültige positive ID
        Assert.AreEqual("approved", records[0].Decision.Status);      // Die gespeicherte Entscheidung lautet approved
    }
}
