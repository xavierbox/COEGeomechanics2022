using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.DomainObject;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GeoTemplateImages = Slb.Ocean.Petrel.PetrelProject.WellKnownTemplates.GeomechanicGroup;
using Templates = Slb.Ocean.Petrel.PetrelProject.WellKnownTemplates;

namespace Gigamodel.OceanUtils
{
    //this is really horrible. All of it.
    internal class TemplateUtils
    {
        public static List<Bitmap> BitmapForNamesSimilarTo( List<string> l )
        {
            List<Bitmap> bitmaps = new List<Bitmap>();
            List<Bitmap> images = ImagesForKnownPreffixes;

            foreach (string s in l)
            {
                int index = -1;
                for (int n = 0; n < _groupPreffixes.Count(); n++)
                {
                    if (s.Contains(_groupPreffixes[n]))
                    {
                        index = n;
                        break;
                    }

                    bitmaps.Add(n >= 0 ? ImagesForKnownPreffixes[n] : DefaultImage);
                }
            }
            return bitmaps;
        }

        public static List<Template> TemplateForNamesSimilarTo( List<string> l )
        {
            List<Template> templatesKnown = TemplatesForKnownPreffixes;
            List<Template> templatesGuessed = new List<Template>();

            foreach (string s in l)
            {
                int index = -1;
                for (int n = 0; n < _groupPreffixes.Count(); n++)
                {
                    if (s.Contains(_groupPreffixes[n]))
                    {
                        index = n;

                        break;
                    }
                }

                templatesGuessed.Add(index >= 0 ? templatesKnown[index] : DefaultTemplate);
            }
            return templatesGuessed;
        }

        private static string[] _groupPreffixes = { "ROCKDIS", "EFFSTR", "TOTSTR", "PRESS", "STRAIN" };

        private static Bitmap DefaultImage
        {
            get
            {
                return PetrelSystem.TemplateService.GetTemplateTypeImage(Templates.MiscellaneousGroup.General.TemplateType);
            }
        }

        //this is really horrible.
        private static List<Bitmap> ImagesForKnownPreffixes
        {
            get
            {
                List<Bitmap> images = new List<Bitmap>();
                images.AddRange(new Bitmap[]
                {
                    PetrelSystem.TemplateService.GetTemplateTypeImage( GeoTemplateImages.RockDisplacement.TemplateType),
                    PetrelSystem.TemplateService.GetTemplateTypeImage( GeoTemplateImages.StressEffective.TemplateType),
                    PetrelSystem.TemplateService.GetTemplateTypeImage( GeoTemplateImages.StressTotal.TemplateType),
                    PetrelSystem.TemplateService.GetTemplateTypeImage( Templates.PetrophysicalGroup.Pressure.TemplateType),
                    PetrelSystem.TemplateService.GetTemplateTypeImage( GeoTemplateImages.Strain.TemplateType),
                    TemplateUtils.DefaultImage
                });

                return images;
            }
        }

        private static List<Template> TemplatesForKnownPreffixes
        {
            get
            {
                return new List<Template>()
                {
                PetrelProject.WellKnownTemplates.GeomechanicGroup.RockDisplacement,
                PetrelProject.WellKnownTemplates.GeomechanicGroup.StressEffective,
                PetrelProject.WellKnownTemplates.GeomechanicGroup.StressTotal,
                PetrelProject.WellKnownTemplates.PetrophysicalGroup.Pressure,
                PetrelProject.WellKnownTemplates.GeomechanicGroup.Strain
                };
            }
        }

        private static Template DefaultTemplate
        {
            get
            {
                return PetrelProject.WellKnownTemplates.MiscellaneousGroup.General;
            }
        }

        //public static Image GetImage(string name)
        //{
        //    ImageList images = new ImageList();

        //    images.Images.AddRange(new Image[]
        //        {
        //        PetrelSystem.TemplateService.GetTemplateTypeImage( GeoTemplateImages.RockDisplacement.TemplateType)

        //        });
        //}
        ////images.Images.AddRange(new Image[] {
        ////    PetrelSystem.TemplateService.GetTemplateTypeImage( GeoTemplateImages.RockDisplacement.TemplateType),
        ////    PetrelSystem.TemplateService.GetTemplateTypeImage( GeoTemplateImages.StressEffective.TemplateType),
        ////    PetrelSystem.TemplateService.GetTemplateTypeImage( GeoTemplateImages.StressTotal.TemplateType),
        ////    PetrelSystem.TemplateService.GetTemplateTypeImage( Templates.PetrophysicalGroup.Pressure.TemplateType),
        ////    PetrelSystem.TemplateService.GetTemplateTypeImage( GeoTemplateImages.Strain.TemplateType),
        ////    PetrelSystem.TemplateService.GetTemplateTypeImage( Templates.MiscellaneousGroup.General.TemplateType)

        ////    });

        ////Bitmap GetTemplateImage(string preffix)
        ////{
        ////}
    }
}