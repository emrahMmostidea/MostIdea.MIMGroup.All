using System;
using System.Threading.Tasks;

namespace MostIdea.MIMGroup.Storage
{
    public interface IBinaryObjectManager
    {
        Task<BinaryObject> GetOrNullAsync(Guid id);

        Task<BinaryObject> GetFileWithRowId(Guid RowId, string TableName);
        
        Task SaveAsync(BinaryObject file);
        
        Task DeleteAsync(Guid id);
    }
}