using BLL.IServices;
using BLL.Services.AdminSide;
using Common.Model.AdminSide;

namespace API
{
    public class ServicesMapping
    {
        public ServicesMapping(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<
                IUsersService<List<CreateUserArgs>, List<ReadUserArgs>, List<UpdateUserArgs>, List<int?>>,
                UsersService
            >();
            builder.Services.AddScoped<
                IQuestionsService<List<CreateQuestionArgs>, List<ReadQuestionArgs>, List<UpdateQuestionArgs>, List<int?>>,
                QuestionsService
            >();
            builder.Services.AddScoped<
                IAnswerQuestionsService<List<CreateAnswerQuestionArgs>, List<ReadAnswerQuestionArgs>, List<UpdateAnswerQuestionArgs>, List<int?>>,
                AnswerQuestionsService
                >();
            builder.Services.AddScoped<
                IAssetsService<List<CreateAssetArgs>, List<ReadAssetArgs>, List<UpdateAssetArgs>, List<int?>>,
                AssetsService
            >();
            builder.Services.AddScoped<
                IAssetCategorysService<List<CreateAssetCategoryArgs>, List<ReadAssetCategoryArgs>, List<UpdateAssetCategoryArgs>, List<int?>>,
                AssetCategorysService
            >();
            builder.Services.AddScoped<
                ICardsService<List<CreateCardArgs>, List<ReadCardArgs>, List<UpdateCardArgs>, List<int?>>,
                CardsService
            >();
            builder.Services.AddScoped<
                ICardEffectsService<List<CreateCardEffectArgs>, List<ReadCardEffectArgs>, List<UpdateCardEffectArgs>, List<int?>>,
                CardEffectsService
            >();
            builder.Services.AddScoped<
                ICashFlowsService<List<CreateCashFlowArgs>, List<ReadCashFlowArgs>, List<UpdateCashFlowArgs>, List<int?>>,
                CashFlowsService
            >();
            builder.Services.AddScoped<
                ICashFlowCategorysService<List<CreateCashFlowCategoryArgs>, List<ReadCashFlowCategoryArgs>, List<UpdateCashFlowCategoryArgs>, List<int?>>,
                CashFlowCategorysService
            >();
            builder.Services.AddScoped<
                IEffectTablesService<List<CreateEffectTableArgs>, List<ReadEffectTableArgs>, List<UpdateEffectTableArgs>, List<int?>>,
                EffectTablesService
            >();
            builder.Services.AddScoped<
                IFunctionsService<List<CreateFunctionArgs>, List<ReadFunctionArgs>, List<UpdateFunctionArgs>, List<int?>>,
                FunctionsService
            >();
            builder.Services.AddScoped<
                ILogsService<List<CreateLogArgs>, List<ReadLogArgs>, List<UpdateLogArgs>, List<int?>>,
                LogsService
            >();
            builder.Services.AddScoped<
                IQustionEffectsService<List<CreateQustionEffectArgs>, List<ReadQustionEffectArgs>, List<UpdateQustionEffectArgs>, List<int?>>,
                QustionEffectsService
            >();
            builder.Services.AddScoped<
                IRolesService<List<CreateRoleArgs>, List<ReadRoleArgs>, List<UpdateRoleArgs>, List<int?>>,
                RolesService
            >();
            builder.Services.AddScoped<
                IRoleFunctionsService<List<CreateRoleFunctionArgs>, List<ReadRoleFunctionArgs>, List<UpdateRoleFunctionArgs>, List<int?>>,
                RoleFunctionsService
            >();
            builder.Services.AddScoped<
                IUsersService<List<CreateUserArgs>, List<ReadUserArgs>, List<UpdateUserArgs>, List<int?>>,
                UsersService
            >();
            builder.Services.AddScoped<
                IUserBoardsService<List<CreateUserBoardArgs>, List<ReadUserBoardArgs>, List<UpdateUserBoardArgs>, List<int?>>,
                UserBoardsService
            >();
        }
    }
}
