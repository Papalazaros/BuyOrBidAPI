using BuyOrBid.Models;
using BuyOrBid.Models.Database;
using BuyOrBid.Models.Database.Enums;
using BuyOrBid.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BuyOrBid
{
    public class DatabaseSeedHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Random random = new Random();

        public DatabaseSeedHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using IServiceScope scope = _serviceProvider.CreateScope();

            //await UpdateMakesAndModels(scope);
            await CreateTestData(scope);
        }

        private int? GetRandomEnumValue(Type type)
        {
            Array availableEnums = Enum.GetValues(type);
            return (int)availableEnums.GetValue(random.Next(0, availableEnums.Length))!;
        }

        private async Task CreateTestData(IServiceScope scope)
        {
            IAutoPostService autoService = scope.ServiceProvider.GetRequiredService<IAutoPostService>();
            IPostService postService = scope.ServiceProvider.GetRequiredService<IPostService>();
            MyDbContext myDbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();

            List<string> vins = new List<string>
            {
                "YV149MTS6G2409804",
                "JTJYWRBZ6G2009198",
                "SALWG2VF5GA642581",
                "YV126MEB3F1186381",
                "YV1902TH6F2304610",
                "JA4JZ4AX9FZ001840",
                "WMWZP3C56FT298666",
                "KNAFZ4A81F5290432",
                "WA1CMCFE3FD080332",
                "3VW2M1AJ5EM308034",
                "WMWSX1C50ET623235",
                "WDDHH8JB5EA799727",
                "JTHBE5D25E5999999",
                "JTHBE5D21E5009987",
                "WAUWFBFL0E0015575",
                "JM3TB3BV1D0402771",
                "JM1BL1UF2D1843458",
                "JTHCF5C2XD2036051",
                "1C4BJWKGXDL508578",
                "WBA3A5C5XDF255852",
                "WAUBGCFC1DN070779",
                "3VWFP7AT0CM615492",
                "JM1BL1V74C1654199",
                "SALSF2D48CA745019",
                "5UXWX5C51CC727594",
                "WBSDX9C50CE632859",
                "WBAKC8C57CCC34760",
                "WAUFEAFMXCA868546",
                "JN8AS5MT9BW568025",
                "JN1AR5EF2BM240198",
                "JA32Y6HV1BU003147",
                "JTHFF2C26B2515141",
                "1J4N72GU9BD117409",
                "SAJWA2GEXBMV00832",
                "1GC4CZC80BF120492",
                "WBSKG9C57BE369243",
                "YV4852CZ0A1550673",
                "3N1AB6AP4A1705397",
                "WMWMS3C45ATY08384",
                "WDDGF5EB5AR097221",
                "5NMSK3BB0A0011955",
                "KMHCN4BC5AU476036",
                "19XFA1E39AE028694",
                "1FTPX1EV0A0017433",
                "1G1YR2DW4A50008EX",
                "WB1024400AZR12312",
                "KL2TX56E49B323948",
                "JA3AY68V09U612302",
                "2LMDU68L29BJ11359",
                "KMHGC46E394061991",
                "KL1TJ53779B616366",
                "WBXPC934X9WJ30129",
                "WUADA64F19N900439",
                "5J8TB18219A802665",
                "2G2WP552681193952",
                "KL2TJ51608B202365",
                "KMHEU41DX8A516824",
                "1Z7HT38K87S100173",
                "KMHJM12B070013219",
                "1ZVFT80N475211367",
                "1FTPX14587KA80202",
                "1FDSE35S57DB34927",
                "3D2WS26D07G777824",
                "3D6WS26D67G828192",
                "WB10397017ZP61849",
                "2HNYD28377H522352",
                "WVWCU93C86P085276",
                "KL2TD69656B612870",
                "WMWRF33456TG14703",
                "WDBTK56F36T072574",
                "1LNFM83V060018247",
                "SALMH13446A220123",
                "KNALD221565103572",
                "2HKYF286060012701",
                "1HGCM56766A041742",
                "WBAVD13596KX00407",
                "6G2VX12U95L553833",
                "WMWRC33444T050912",
                "1J4GL48K246250341",
                "1HD1BMY1X4Y010673",
                "JH4CL96974CD14071",
                "5GZCZ23D13S847842",
                "KNDUP131936358464",
                "KNDUP131336425950",
                "1HD1JAB313Y039358",
                "4F4YR12U72TM08759",
                "JHLRD78512C024509",
                "1HD1PDC392Y952267",
                "2B4GT54L62R753223",
                "JF1SF63501H759113",
                "SALTY12471A702896",
                "1FAFP40491FS62183",
                "YV1VW255XYFU88185",
                "WBAEJ1341YA382128",
                "2HHMB4660YH904692",
                "1LNFM81W7XY601946",
                "1GCGC33R3XF100201",
                "JT8BF28G1W5630798",
                "JH4KA9653WC800195",
                "WDBGA51E4TA328716",
                "1LNLM82W9TY705110",
                "KNDJA723XT5517217",
                "2GCEK19R3T1146294",
                "WBABK7320TET60222",
                "JA3AP47H3SY018398",
                "SALHF1349SA654345",
                "JH4DC4340SS001220",
                "4T1SK12E7PU300490",
                "JM2UF2138N0251579",
                "1B7FG23Y0N0012403",
                "1HGEE4860L0017233",
                "1HD4CAM33LY118860",
                "1FTEF25M0L0016339",
                "3VWBB91G0K0016657",
                "JT2MX83E2K0030681",
                "2G3AM51NXK2312943",
                "2G3AM51N0K2395928",
                "WDBEA26E6KB002021",
                "1LNCM9744KY646690",
                "1FTCR10T4KUC34284",
                "1FTCR10A9KUC55386",
                "2G3AM513XJ2364151",
                "1G3AJ513xJG314327",
                "1LNBM9741JY839714",
                "1FTCR14T6JPB25476",
                "1FTBR10A7JUD45386",
                "JB3BA24K0JU001772",
                "WVWCA0157HK036627",
                "JT2EL31D8H0101727",
                "2G3AJ51W1H9422815",
                "1G3AJ5131HD398220",
                "JN1HU1110HT263333",
                "1JTBG64S0H0014395",
                "1FTBR10C3HUB08213",
                "JH4KA2640HC004148",
                "1G3AM19X8GG308517",
                "1G3AJ19X1GG341898",
                "1G3AJ19W6GG420717",
                "1N6HD16Y1GC442608",
                "2B4FK51G8GR605541",
                "1G3AJ19E0FG390717",
                "1G3AM19E8FG344687",
                "1JCBS7440F0013895",
                "1FACP22X2FK213033",
                "1FMEU15HXFLA29734",
                "1G3AM19E4ED315574",
                "1G3AJ19E7EG320727",
                "JN1HK04S0E0010739",
                "WDBDA24A4EA027134",
                "WDBD022A7EA060323",
                "1FABP0521ET102126",
                "1JCNE16N6CT044957",
                "1HD1AJK23CY024920",
                "WVWDG21K9BW201730",
                "WDBAA33A8BB093914",
            };

            var model = myDbContext.Models.Include(x => x.Make).FirstOrDefault();
            var makeId = model.MakeId;

            foreach (string vin in vins)
            {
                AutoPost autoPost = await autoService.CreatePostFromVin(vin);

                DateTime now = DateTime.UtcNow;

                autoPost.AutoCondition ??= (AutoCondition)GetRandomEnumValue(typeof(AutoCondition))!;
                autoPost.AutoType ??= (AutoType)GetRandomEnumValue(typeof(AutoType))!;
                autoPost.Doors ??= 4;
                autoPost.DriveType ??= (DriveType)GetRandomEnumValue(typeof(DriveType))!;
                autoPost.FuelType ??= (FuelType)GetRandomEnumValue(typeof(FuelType))!;
                autoPost.IsPublic = true;
                autoPost.Make ??= model.Make;
                autoPost.MakeId ??= makeId;
                autoPost.Model ??= model;
                autoPost.ModelId ??= model.ModelId;
                autoPost.ModifiedDate = now;
                autoPost.CreatedDate = now;
                autoPost.Color ??= "Black";
                autoPost.CreatedByUserId = 1;
                autoPost.TitleStatus ??= (TitleStatus)GetRandomEnumValue(typeof(TitleStatus))!;
                autoPost.TransmissionType ??= (TransmissionType)GetRandomEnumValue(typeof(TransmissionType))!;
                autoPost.Description = "Test";
                autoPost.Price = 10;
                autoPost.SystemTitle ??= AutoPostService.GenerateTitle(autoPost);
                autoPost.UserTitle = autoPost.SystemTitle;
                autoPost.Language = "English";
                autoPost.IsPublic = true;

                await postService.Create(autoPost);
            }
        }

        private async Task UpdateMakesAndModels(IServiceScope scope)
        {
            MyDbContext myDbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();

            IAutoDataService autoDataService = scope.ServiceProvider.GetRequiredService<IAutoDataService>();

            IEnumerable<Make> makes = await autoDataService.GetMakes();

            IEnumerable<Make> newMakes = (from fromApi in makes
                                          join fromDb in myDbContext.Makes on fromApi.VpicId equals fromDb.VpicId into tempJoin
                                          from joinResult in tempJoin.DefaultIfEmpty()
                                          select new { fromApi, joinResult }).Where(x => x.joinResult == null).Select(x => x.fromApi);

            List<Task> modelsTask = new List<Task>();
            HashSet<int> makesToRemove = new HashSet<int>();

            foreach (Make make in makes)
            {
                IEnumerable<Model> models = await autoDataService.GetModels(make);

                if (models.Count() <= 1)
                {
                    makesToRemove.Add(make.VpicId!.Value);
                    continue;
                }

                IEnumerable<Model> newModels = (from fromApi in models
                                                join fromDb in myDbContext.Models on fromApi.VpicId equals fromDb.VpicId into tempJoin
                                                from joinResult in tempJoin.DefaultIfEmpty()
                                                select new { fromApi, joinResult }).Where(x => x.joinResult == null).Select(x => x.fromApi);

                modelsTask.Add(myDbContext.Models.AddRangeAsync(newModels));
            }

            newMakes = newMakes.Where(x => !makesToRemove.Contains(x.VpicId!.Value));

            await Task.WhenAll(modelsTask);
            await myDbContext.Makes.AddRangeAsync(newMakes);
            await myDbContext.SaveChangesAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
