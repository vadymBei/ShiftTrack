using ShiftTrack.Application.Common.Behaviours;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Core.Unit.Tests.Application.Common.UseCases.Commands;
using ShiftTrack.Core.Unit.Tests.Application.Common.UseCases.Queries;

namespace ShiftTrack.Core.Unit.Tests.Application.Common
{
    public class TransactionBehaviourTests
    {
        private readonly FakeUnitOfWork _unitOfWork = new();

        [Fact]
        public async Task Command_RunsInTransaction()
        {
            var behaviour = new TransactionBehaviour<FakeCommand, string>(_unitOfWork);

            var result = await behaviour.Handle(new FakeCommand(), _ => Task.FromResult("done"), CancellationToken.None);

            Assert.Equal("done", result);
            Assert.Equal(1, _unitOfWork.Transactions);
        }

        [Fact]
        public async Task Query_RunsWithoutTransaction()
        {
            var behaviour = new TransactionBehaviour<FakeQuery, string>(_unitOfWork);

            var result = await behaviour.Handle(new FakeQuery(), _ => Task.FromResult("done"), CancellationToken.None);

            Assert.Equal("done", result);
            Assert.Equal(0, _unitOfWork.Transactions);
        }

        [Fact]
        public async Task CommandWithoutResult_RunsInTransaction()
        {
            var behaviour = new TransactionBehaviour<FakeCommand>(_unitOfWork);
            var handled = false;

            await behaviour.Handle(new FakeCommand(), _ =>
            {
                handled = true;
                return Task.CompletedTask;
            }, CancellationToken.None);

            Assert.True(handled);
            Assert.Equal(1, _unitOfWork.Transactions);
        }

        private sealed class FakeUnitOfWork : IUnitOfWork
        {
            public int Transactions { get; private set; }

            public Task<T> ExecuteInTransaction<T>(Func<Task<T>> action, CancellationToken cancellationToken)
            {
                Transactions++;
                return action();
            }
        }
    }
}

// The behaviour recognises commands by the "*.UseCases.Commands" namespace convention.
namespace ShiftTrack.Core.Unit.Tests.Application.Common.UseCases.Commands
{
    public record FakeCommand : ShiftTrack.Kernel.CQRS.Interfaces.IRequest<string>;
}

namespace ShiftTrack.Core.Unit.Tests.Application.Common.UseCases.Queries
{
    public record FakeQuery : ShiftTrack.Kernel.CQRS.Interfaces.IRequest<string>;
}
