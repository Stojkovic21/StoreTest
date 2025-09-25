using System.Text;
using System.Text.Json.Nodes;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

[TestFixture]
public class UserContollerTests:PlaywrightTest
{
    private IAPIRequestContext Request;

        [SetUp]
        public async Task SetUp()
        {
            var headers = new Dictionary<string, string>
            {
                {"Accept", "applicaiton/json"},
            };

            Request = await Playwright.APIRequest.NewContextAsync(new()
            {
                BaseURL = "http://localhost:5173",
                ExtraHTTPHeaders = headers,
                IgnoreHTTPSErrors = true
            });
        }

    [Test]
    public async Task PostTable_ValidData_ReturnsCreatedAtAction()
    {
        await using var browser = await Playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false // ili true ako hoćeš da ne prikazuje browser
            });

            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();

            // 2️⃣ Otvori stranicu signup
            await page.GotoAsync("http://localhost:5173/signup");

            // 3️⃣ Interceptuj response koji frontend šalje
            var responseTask = page.WaitForResponseAsync(resp =>
                resp.Url.Contains("/customer/signup") && resp.Status == 200);

            // 4️⃣ Popuni polja forme
            await page.FillAsync("input[name='email']", "vanjaaaaa@kkpar.rs");
            await page.FillAsync("input[name='password']", "NovaSifra");
            await page.FillAsync("input[name='ime']", "Vanja");
            await page.FillAsync("input[name='prezime']", "Marinkovc");
            await page.FillAsync("input[name='brTel']", "069696969");

            // 5️⃣ Klikni dugme submit
            await page.ClickAsync("button[type='submit']");

            // 6️⃣ Sačekaj da frontend request završi
            var response = await responseTask;

            // 7️⃣ Uzmi body response-a
            var bodyText = await response.TextAsync();
            Console.WriteLine("Signup response body: " + bodyText);

            // 8️⃣ Parsiraj JSON response
            var json = JsonNode.Parse(bodyText)!;
            var returnedId = json["userId"]!.GetValue<int>();

            // 9️⃣ Assert response id
            Assert.That(returnedId, Is.EqualTo(0));

            // 🔟 Proveri da li je frontend redirectovao korisnika
            // await page.WaitForURLAsync("**/dashboard");
            // Assert.That(page.Url, Does.Contain("/dashboard"));

            // // 1️⃣1️⃣ Proveri localStorage token
            // var token = await page.EvaluateAsync<string>("() => localStorage.getItem('token')");
            // Assert.IsNotNull(token);
            // Assert.IsNotEmpty(token);

            // // 1️⃣2️⃣ Zatvori context i browser
             await context.CloseAsync();
             await browser.CloseAsync();
    }

}