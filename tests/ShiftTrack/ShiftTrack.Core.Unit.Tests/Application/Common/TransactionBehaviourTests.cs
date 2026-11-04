using ShiftTrack.Application.Common.Behaviours;
using ShiftTrack.Core.Unit.Tests.Application.Common.UseCases.Commands;
using ShiftTrack.Core.Unit.Tests.Application.Common.UseCases.Queries;
using ShiftTrack.Core.Unit.Tests.Fakes;

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

        [Fact]
        public async Task AfterCommit_RunsWhenTheCommandSucceeds()
        {
            var behaviour = new TransactionBehaviour<FakeCommand, string>(_unitOfWork);
            var sent = false;

            await behaviour.Handle(new FakeCommand(), async _ =>
            {
                await _unitOfWork.AfterCommit(() =>
                {
                    sent = true;
                    return Task.CompletedTask;
                });

                Assert.False(sent);
                return "done";
            }, CancellationToken.None);

            Assert.True(sent);
        }

        [Fact]
        public async Task AfterCommit_IsDroppedWhenTheCommandFails()
        {
            var behaviour = new TransactionBehaviour<FakeCommand, string>(_unitOfWork);
            var sent = false;

            await Assert.ThrowsAsync<InvalidOperationException>(() => behaviour.Handle(new FakeCommand(), async _ =>
            {
                await _unitOfWork.AfterCommit(() =>
                {
                    sent = true;
                    return Task.CompletedTask;
                });

                throw new InvalidOperationException();
            }, CancellationToken.None));

            Assert.False(sent);
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
