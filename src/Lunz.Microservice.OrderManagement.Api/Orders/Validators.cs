using FluentValidation;
using FluentValidation.Validators;
using Lunz.Data;
using Lunz.Microservice.Common;
using Lunz.Microservice.OrderManagement.CommandStack.Domain.Repositories;
using System;

namespace Lunz.Microservice.OrderManagement.Api.Orders
{
    public class CreateValidator : AbstractValidator<Create.Command>
    {
        public CreateValidator()
        {
            //RuleFor(x => x.HearFrom).InclusiveBetween((short)1, (short)3)
            //    .WithMessage("值必须在 1 - 3 之间。");
            RuleFor(x=>x.Subject).MaximumLength(50)
                .WithMessage("摘要的长度不能大于50个字符");
            RuleFor(x => x.Amount).Must((x, token) => x.Amount <= 10000000)
                .WithMessage("数量不能大于 10000000。");
            RuleFor(x => x.Price).Must((x, token) => x.Price <= 10000000)
                .WithMessage("单价不能大于 10000000。");
            RuleFor(x => x.Date).GreaterThan(new DateTime(2000, 1, 1));
            RuleFor(x => x).Must((x, token) => x.Price >= 0)
                .WithMessage("价格不能小于 0。");
        }
    }

    public class UpdateValidator : AbstractValidator<Update.Command>
    {
        protected readonly IDatabaseScopeFactory _databaseScopeFactory;
        private readonly IOrderRepository _repository;

        public UpdateValidator(IDatabaseScopeFactory databaseScopeFactory,
            IOrderRepository repository)
        {
            _databaseScopeFactory = databaseScopeFactory;
            _repository = repository;


            //RuleFor(x => x.HearFrom).InclusiveBetween((short)1, (short)3)
            //    .WithMessage("值必须在 1 - 3 之间。");
            RuleFor(x => x.Subject).MaximumLength(50)
                .WithMessage("摘要的长度不能大于50个字符。");
            RuleFor(x => x.Amount).Must((x, token) => x.Amount <= 10000000)
                .WithMessage("数量不能大于 10000000。");
            RuleFor(x => x.Price).Must((x, token) => x.Price <= 10000000)
                .WithMessage("单价不能大于 10000000。");
            RuleFor(x => x.Date).GreaterThan(new DateTime(2000, 1, 1));
            RuleFor(x => x).Must((x, token) => x.Price >= 0)
                .WithMessage("价格不能小于 0。");

            RuleFor(x => x).Custom(Validate);
        }

        private void Validate(Update.Command message, CustomContext context)
        {
            var model = _repository.FindAsync(message.Id).GetAwaiter().GetResult();
            if (model == null)
            {
                context.AddNotFoundError("数据不存在");
            }
            else
            {
                if (message.Subject.Equals(model.Subject)
                    && message.Date == model.Date
                    && message.HearFromId == model.HearFromId
                    && message.HearFromName.Equals(model.HearFromName)
                    && message.Amount == model.Amount
                    && message.Price == model.Price
                    && message.Total == model.Total)
                {
                    context.AddNoContentError("数据未更改");
                }
            }
        }

    }

    public class DeleteValidator : AbstractValidator<Delete.Command>
    {
        protected readonly IDatabaseScopeFactory _databaseScopeFactory;
        private readonly IOrderRepository _repository;

        public DeleteValidator(IDatabaseScopeFactory databaseScopeFactory,
            IOrderRepository repository)
        {
            _databaseScopeFactory = databaseScopeFactory;
            _repository = repository;

            RuleFor(x => x).Custom(Validate);
        }

        private void Validate(Delete.Command message, CustomContext context)
        {
            var model = _repository.FindAsync(message.Id.Value).GetAwaiter().GetResult();
            if (model == null)
            {
                context.AddNotFoundError("数据不存在");
            }
        }

    }


}