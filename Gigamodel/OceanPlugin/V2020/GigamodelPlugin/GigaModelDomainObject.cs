using Gigamodel.Data;
using Slb.Ocean.Petrel.UI;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Gigamodel
{
    public class GigamodelDomainObject : IImageInfoSource, INameInfoSource
    {
        private Bitmap _bitmap = PetrelImages.Model_32;
        private DefaultImageInfo _image;
        private NameInfo _nameInfo = null;
        private List<Object> l;

        public GigamodelDomainObject()
        {
            _image = null;
            Name = "Gigamodel";
            l = new List<object>();
            GigaModelDataModel = null;
        }

        public GigaModelDataModel GigaModelDataModel { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }

        #region ImagEInfo interface

        public ImageInfo ImageInfo
        {
            get { if (_image == null) _image = new DefaultImageInfo(_bitmap); return _image; }
        }

        #endregion ImagEInfo interface



        #region NameInfo interface

        public NameInfo NameInfo
        {
            get
            {
                if (_nameInfo == null)
                    _nameInfo = new DefaultNameInfo("GigaModel", " 3D Seismic Giga Model", "GigaModel");
                return _nameInfo;
            }
        }

        #endregion NameInfo interface



        #region IExtensions interface

        public void Add( object obj )
        {
            ;// throw new NotImplementedException();
        }

        public int Count
        {
            get
            {
                return 1;
            }
        }

        public bool Contains( object obj )
        {
            return false;// throw new NotImplementedException();
        }

        public bool Remove( object obj )
        {
            return true;// throw new NotImplementedException();
        }

        #endregion IExtensions interface
    };
}