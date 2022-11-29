using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TMAPIConsume.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace TMAPIConsume.Controllers
{
    public class HomeController : Controller
    {

        public async Task<IActionResult> Index()
        {
            List<Toolmark> toolMarkList = new List<Toolmark>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://EMWEBAPI/api/Toolmark/GetToolmark"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    toolMarkList = JsonConvert.DeserializeObject<List<Toolmark>>(apiResponse);
                }
            }
            return View(toolMarkList);
        }


        public ViewResult GetToolmark() => View();

        [HttpPost]
        public async Task<IActionResult> GetToolmark(string id)
        {
            Toolmark toolmark = new Toolmark();
            List<Toolmark> toolmarkList = new List<Toolmark>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://EMWEBAPI/api/Toolmark/GetToolmark/ToolmarkId=" + id))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        toolmarkList = JsonConvert.DeserializeObject<List<Toolmark>>(apiResponse);

                        for(int i = 0; i< toolmarkList.Count; i++)
                        {
                            toolmark = toolmarkList[i];

                        }


                    }
                    else
                        ViewBag.StatusCode = response.StatusCode;
                }
            }
            return View(toolmark);
        }


        public ViewResult AddToolmark() => View();

        [HttpPost]
        public async Task<IActionResult> AddToolmark(Toolmark toolmark)
        {
            Toolmark receivedToolmark = new Toolmark();
            List<Toolmark> receivedToolmarkList = new List<Toolmark>();

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(toolmark), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("https://EMWEBAPI/api/Toolmark/AddToolmark", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    receivedToolmarkList = JsonConvert.DeserializeObject<List<Toolmark>>(apiResponse);
                    for (int i = 0; i < receivedToolmarkList.Count; i++)
                    {
                        receivedToolmark = receivedToolmarkList[i];

                    }
                }
            }
            return View(receivedToolmark);
        }

        public async Task<IActionResult> UpdateToolmark(int id)
        {
            Toolmark toolmark = new Toolmark();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost/api/Toolmark/UpdateById/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    toolmark = JsonConvert.DeserializeObject<Toolmark>(apiResponse);
                }
            }
            return View(toolmark);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateToolmark(int ID, Toolmark toolmark)
        {
            Toolmark receivedToolmark = new Toolmark();
            using (var httpClient = new HttpClient())
            {
                var content = new MultipartFormDataContent();

                content.Add(new StringContent(toolmark.CaseNumber), "CaseNumber");
                content.Add(new StringContent(toolmark.Category), "Category");
                content.Add(new StringContent(toolmark.Email), "Email");
                content.Add(new StringContent(toolmark.Note), "Note");
                content.Add(new StringContent(toolmark.ImageFileName), "ImageFileName");
                content.Add(new StringContent(toolmark.DateOfCollected), "DateOfCollected");

                using (var response = await httpClient.PutAsync("https://EMWEBAPI/api/Toolmark/UpdateToolmark/" + ID.ToString() , content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ViewBag.Result = "Success";
                    receivedToolmark = JsonConvert.DeserializeObject<Toolmark>(apiResponse);
                }
            }
            return View(receivedToolmark);
        }

        public async Task<IActionResult> UpdateToolmarkPatch(int id)
        {
            Toolmark toolmark = new Toolmark();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://EMWEBAPI/api/Toolmark/UpdateToolmark/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    toolmark = JsonConvert.DeserializeObject<Toolmark>(apiResponse);
                }
            }
            return View(toolmark);
        }

        private Toolmark FilterValueImport(Toolmark toolmark)
        {
            if (toolmark.CaseNumber == null)
                toolmark.CaseNumber = "";

            if (toolmark.Category == null)
                toolmark.Category = "";

            if (toolmark.Email == null)
                toolmark.Email = "";

            if (toolmark.DateOfCollected == null)
                toolmark.DateOfCollected = System.DateTime.Now.ToShortDateString();

            if (toolmark.ImageFileName == null)
                toolmark.ImageFileName = "";

            if (toolmark.Note == null)
                toolmark.Note = "";

            return toolmark;
        }


        [HttpPost]
        public async Task<IActionResult> UpdateToolmarkPatch(int id, Toolmark toolmark)
        {
            Toolmark oToolmark = FilterValueImport(toolmark);
            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri("https://EMWEBAPI/api/Toolmark/UpdateToolmark/" + id),
                    Method = new HttpMethod("Put"),
                    Content = new StringContent("{ \"CaseNumber\": \"" + oToolmark.CaseNumber
                    + "\",\"Category\": \"" + oToolmark.Category
                    + "\", \"Email\": \"" + oToolmark.Email
                    + "\",\"DateOfCollected\": \"" + oToolmark.DateOfCollected
                    + "\",\"ImageFileName\": \"" + oToolmark.ImageFileName
                    + "\",\"Note\": \"" + oToolmark.Note
                    + "\"}"
                    , Encoding.UTF8, "application/json")
                };

                var response = await httpClient.SendAsync(request);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteToolmark(int ToolmarkId)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("https://EMWEBAPI/api/Toolmark/" + ToolmarkId))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }

            return RedirectToAction("Index");
        }

    }
}
