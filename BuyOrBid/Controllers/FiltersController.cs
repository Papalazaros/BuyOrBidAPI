using BuyOrBid.Models;
using BuyOrBid.Models.Database;
using BuyOrBid.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BuyOrBid.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FiltersController : ControllerBase
    {
        private readonly IAutoPostService _autoService;

        public FiltersController(IAutoPostService autoService)
        {
            _autoService = autoService;
        }

        [HttpGet]
        public IActionResult GetFilterModels()
        {
            List<FilterDescriptor> filterDescriptors = new List<FilterDescriptor>();

            foreach (PropertyInfo property in typeof(AutoFilterRequest).GetProperties())
            {
                Type propertyType = property.PropertyType;
                Type propertySubType = property.PropertyType.GenericTypeArguments.FirstOrDefault();

                FilterDescriptor filterDescriptor = new FilterDescriptor();
                filterDescriptor.PropertyName = property.Name;
                filterDescriptor.PropertyType = propertyType.Name;
                filterDescriptor.PropertySubType = propertySubType?.Name;

                if (propertyType.IsEnum)
                {
                    filterDescriptor.AvailableValues = Enum.GetValues(propertyType);
                }
                else if (propertySubType != null && propertySubType.IsEnum)
                {
                    filterDescriptor.AvailableValues = Enum.GetValues(propertySubType);
                }
                else if (property.Name == "CylinderCount")
                {
                    filterDescriptor.AvailableValues = new int[] { 0, 1, 2, 3, 4, 5, 6, 8, 10, 12 };
                }
                //else if (property.Name == "Makes")
                //{
                //    IEnumerable<Make> makes = await _autoService.GetMakes();
                //    filterDescriptor.AvailableValues = makes.OrderBy(x => x.MakeName).Select(x => new { Key = x.MakeId, Value = x.MakeName });
                //}
                //else if (property.Name == "Models")
                //{
                //    IEnumerable<Model> models = await _autoService.GetModels();
                //    filterDescriptor.AvailableValues = models.OrderBy(x => x.ModelName).Select(x => new { Key = x.ModelId, Value = x.ModelName, DependsOn = new { PropertyName = "Makes", PropertyValue = x.MakeId } });
                //}

                RangeAttribute? rangeAttribute = property.GetCustomAttribute<RangeAttribute>();

                if (rangeAttribute != null)
                {
                    filterDescriptor.Rules = new List<RangeRule>
                    {
                        new RangeRule(rangeAttribute.Minimum, rangeAttribute.Maximum)
                    };
                }

                filterDescriptors.Add(filterDescriptor);
            }

            return Ok(filterDescriptors);
        }
    }
}
