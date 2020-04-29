using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using XGame.Domain.Arguments.Jogador;
using XGame.Domain.Interfaces.Services;


namespace XGame.Api.Security
{
    //
    public class AuthorizationProvider : OAuthAuthorizationServerProvider
    {
        //
        private readonly UnityContainer _container;

        public AuthorizationProvider(UnityContainer container)
        {
            _container = container;
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            //requer segurança vai chamar essa validação
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {

                IServiceJogador serviceJogador = _container.Resolve<IServiceJogador>();

                //obtendo a autenticação, o email vai vir do contexto do oauth
                AutenticarJogadorRequest request = new AutenticarJogadorRequest();
                request.Email = context.UserName;
                request.Senha = context.Password;

                //Autenticando Jogador
                AutenticarJogadorResponse response = serviceJogador.AutenticarJogador(request);

                
                //se jogador é inválido e não encontrou o jogador no banco então é inválido
                if (serviceJogador.IsInvalid())
                {
                    if (response == null)
                    {
                        context.SetError("invalid_grant", "Preencha um e-mail válido e uma senha com pelo menos 6 caracteres.");
                        return;
                    }
                }

                serviceJogador.ClearNotifications();
                
                if (response == null)
                {
                    context.SetError("invalid_grant", "Jogador não encontrado!");
                    return;
                }

                //processo de escrever o token, nesse caso vai criar um clain jogador
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);

                //Definindo as Claims
                identity.AddClaim(new Claim("Jogador", JsonConvert.SerializeObject(response)));

                var principal = new GenericPrincipal(identity, new string[] { });

                Thread.CurrentPrincipal = principal;

                //vai jogar a identidade dentro do contexto validation
                context.Validated(identity);
            }
            catch (Exception ex)
            {
                context.SetError("invalid_grant", ex.Message);
                return;
            }
        }
    }
}
