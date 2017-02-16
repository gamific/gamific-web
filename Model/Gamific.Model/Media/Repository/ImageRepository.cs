using System;
using System.IO;
using System.Linq;
using System.Net.Mime;
using Vlast.Gamific.Model.Media.Domain;
using Vlast.Util.Aws;
using Vlast.Util.Data;
using Vlast.Util.Parameter;

namespace Vlast.Gamific.Model.Media.Repository
{

    /// <summary>
    /// Consulta de dados relacionados com a logo
    /// </summary>
    public class ImageRepository
    {

        public static string BANNER_PHOTOS_S3 = "images/logos/empresas/logo-{0}";

        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile ImageRepository instance;

        private ImageRepository() { }

        public static ImageRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new ImageRepository();
                    }
                }
                return instance;
            }
        }

        #endregion

        /// <summary>
        /// Recupera a imagem
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        public ImageEntity GetById(int imageId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query =
                            from t in context.Images
                            where t.Id == imageId && t.Status == GenericStatus.ACTIVE
                            select t;

                return query.FirstOrDefault();
            }
        }

        /// <summary>
        /// Cria uma nova imagem
        /// </summary>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        public ImageEntity CreateImage(ImageEntity newEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                newEntity.LastUpdate = DateTime.UtcNow;
                context.Images.Attach(newEntity);
                context.Entry(newEntity).State = System.Data.Entity.EntityState.Added;
                context.SaveChanges();
            }

            return newEntity;
        }

        /// <summary>
        /// Query para consulta externa
        /// </summary>
        /// <returns></returns>
        public IQueryable<ImageEntity> GetAll()
        {
            ModelContext context = new ModelContext();
            var query = from sc in context.Images
                        where sc.Status == GenericStatus.ACTIVE
                        orderby sc.Id ascending
                        select sc;

            return query;
        }

        /// <summary>
        /// Atualiza as informações de uma imagem
        /// </summary>
        /// <param name="updatedEntity"></param>
        /// <returns></returns>
        public ImageEntity UpdateImage(ImageEntity updatedEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                updatedEntity.LastUpdate = DateTime.UtcNow;
                context.Images.Attach(updatedEntity);
                context.Entry(updatedEntity).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }

            return updatedEntity;

        }

        /// <summary>
        /// Salva a imagem
        /// </summary>
        /// <param name="imageId"></param>
        /// <param name="photo"></param>
        /// <returns></returns>
        public bool SaveOrReplaceLogo(int imageId, byte[] photo)
        {
            string key = String.Format(BANNER_PHOTOS_S3, imageId);
            return S3Helper.ReplaceS3Object(ParameterCache.S3BUCKET, key, MediaTypeNames.Image.Jpeg, photo);
        }

        /// <summary>
        /// Recupera uma imagem
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        public Stream GetLogo(long imageId)
        {
            string key = String.Format(BANNER_PHOTOS_S3, imageId);
            return S3Helper.GetS3ObjectStream(ParameterCache.S3BUCKET, key);
        }

    }
}
