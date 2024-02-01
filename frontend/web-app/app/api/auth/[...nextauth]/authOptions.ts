import { NextAuthOptions } from "next-auth";
import DuendeIdentityServer6 from "next-auth/providers/duende-identity-server6";

export const authOptions: NextAuthOptions = {
    session: {
        strategy: 'jwt'
    },
    providers: [
        DuendeIdentityServer6({
            id: 'id-server',
            clientId: 'nextApp',
            clientSecret: 'Secret',
            issuer: process.env.AUTH_URL,
            authorization: {params: {scope: 'openid profile auctionService'}},
            idToken: true
        })
    ],
    callbacks: {
        async jwt({token, profile, account}){
            if(profile){
                token.username = profile.user_name;
            }

            if(account){
                token.access_token = account.access_token;
            }
            
            return token;
        },
        
        async session({session, token}) {
            if(token){
                session.user.username = token.username;
            }

            return session;
        },
    }
}