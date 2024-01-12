import NextAuth, { NextAuthOptions } from "next-auth"
import DuendeIdentityServer6 from "next-auth/providers/duende-identity-server6"

export const authOptions: NextAuthOptions = {
    session: {
        strategy: 'jwt'
    },
    providers: [
        DuendeIdentityServer6({
            id: 'id-server',
            clientId: 'nextApp',
            clientSecret: 'Secret',
            issuer: 'http://localhost:5001',
            authorization: {params: {scope: 'openid profile auctionService'}},
            idToken: true
        })
    ],
    callbacks: {
        async jwt({token, profile}){
            if(profile){
                token.username = profile.user_name;
            }

            console.log(token, profile);
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

const handler = NextAuth(authOptions);

export { handler as GET, handler as POST }