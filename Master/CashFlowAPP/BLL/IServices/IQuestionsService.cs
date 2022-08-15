using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IServices
{
    interface IQuestionsService<CreateArgs, ReadArgs, UpdateArgs, DeleteArgs> : ICrudService<CreateArgs, ReadArgs, UpdateArgs, DeleteArgs> { }
}
