using Forte.EpiserverRedirects.Model.RedirectRule;


namespace Forte.EpiserverRedirects.Repository
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity">Store-specific RedirectRule class</typeparam>
    public interface IRedirectRuleMapper<TEntity>
    {
        TEntity ToNewEntity(IRedirectRule item);

        void MapForUpdate(IRedirectRule from, TEntity to);
    }
}
