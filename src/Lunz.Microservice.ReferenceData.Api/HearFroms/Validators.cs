using FluentValidation;
using FluentValidation.Validators;
using Lunz.Data;
using Lunz.Microservice.Core.Models;
using Lunz.Microservice.ReferenceData.Models.Api;
using Lunz.Microservice.ReferenceData.QueryStack.Repositories;
using Lunz.Microservice.Common;

namespace Lunz.Microservice.ReferenceData.Api.HearFroms
{
    public class CreateValidator : AbstractValidator<Create.Command>
    {
        public CreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .WithMessage("名称不能为空。");
        }
    }

    public class UpdateValidator : AbstractValidator<Update.Command>
    {
		protected readonly IDatabaseScopeFactory _databaseScopeFactory;
		private readonly IHearFromRepository _hearFromRepository;

		public UpdateValidator(IDatabaseScopeFactory databaseScopeFactory,
			IHearFromRepository hearFromRepository)
        {
			_databaseScopeFactory = databaseScopeFactory;
			_hearFromRepository = hearFromRepository;

            RuleFor(x => x.Id).NotEmpty()
                .WithMessage("Id 不能为空。");
            RuleFor(x => x.Name).NotEmpty()
                .WithMessage("名称不能为空。");

			RuleFor(x => x).Custom(Validate);
		}

		private void Validate(Update.Command message, CustomContext context)
		{
			using (var scope = _databaseScopeFactory.CreateReadOnly())
			{
				var model = _hearFromRepository.FindAsync(message.Id).GetAwaiter().GetResult();
				if (model == null)
				{
					context.AddNotFoundError("数据不存在");
				}
				else
				{
					if (message.Name.Equals(model.Name))
					{
						context.AddNoContentError("数据未更改");
					}
				}
			}				
		}

	}

	public class DeleteValidator : AbstractValidator<Delete.Command>
	{
		protected readonly IDatabaseScopeFactory _databaseScopeFactory;
		private readonly IHearFromRepository _hearFromRepository;

		public DeleteValidator(IDatabaseScopeFactory databaseScopeFactory,
			IHearFromRepository hearFromRepository)
		{
			_databaseScopeFactory = databaseScopeFactory;
			_hearFromRepository = hearFromRepository;

			RuleFor(x => x).Custom(Validate);
		}

		private void Validate(Delete.Command message, CustomContext context)
		{
			using (var scope = _databaseScopeFactory.CreateReadOnly())
			{
				var model = _hearFromRepository.FindAsync<HearFromDetails>(message.Id.Value).GetAwaiter().GetResult();
				if (model == null)
				{
					context.AddNotFoundError("数据不存在");
				}
			}
						
		}

	}

}
