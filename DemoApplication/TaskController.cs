using DemoApplication.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DemoApplication
{
    [Route("api/[controller]")]
    public class TaskController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var toDoList = new List<ToDoModel>();
                var url = "https://jsonplaceholder.typicode.com/todos";
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var responseMessage = client.GetAsync(url).Result;
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseResult = responseMessage.Content.ReadAsStringAsync().Result;
                        toDoList = JsonConvert.DeserializeObject<List<ToDoModel>>(responseResult);
                        toDoList = toDoList != null && toDoList.Count >= 20
                            ? toDoList.Take(20).ToList() : new List<ToDoModel>();
                        var jsonData = new { data = toDoList };
                        return Ok(jsonData);
                    }
                    else
                    {
                        return Ok(responseMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}
