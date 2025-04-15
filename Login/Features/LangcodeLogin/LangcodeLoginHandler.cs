using Shared.Mediator;

namespace Login.Features.LangcodeLogin
{
    public record LangcodeLoginCommand(string UserId) : ICustomCommand<LangcodeLoginResult>;
    public record LangcodeLoginResult(string Token, DateTime CheckTime);

    internal class LangcodeLoginHandler : ICustomRequestHandler<LangcodeLoginCommand, LangcodeLoginResult>
    {
        public Task<LangcodeLoginResult> Handle(LangcodeLoginCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new LangcodeLoginResult(Guid.NewGuid().ToString(), DateTime.Now));
        }
    }
}