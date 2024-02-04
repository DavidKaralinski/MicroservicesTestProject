import { NextAuthOptions } from "next-auth";
import AzureADB2CProvider from "next-auth/providers/azure-ad-b2c";

export const authOptions: NextAuthOptions = {
  session: {
    strategy: "jwt",
  },
  providers: [
    AzureADB2CProvider({
      id: "id-server",
      tenantId: process.env.AZURE_AD_B2C_TENANT_NAME,
      clientId: process.env.AZURE_AD_B2C_CLIENT_ID!,
      clientSecret: process.env.AZURE_AD_B2C_CLIENT_SECRET!,
      primaryUserFlow: process.env.AZURE_AD_B2C_PRIMARY_USER_FLOW,
      authorization: { params: { scope: "offline_access openid https://microservicestestproject.onmicrosoft.com/auctions-api/auctions.write" } },
      checks: ["pkce"],
      client: {
        token_endpoint_auth_method: "none",
      },
    }),
  ],
  callbacks: {
    async jwt({ token, profile, account }) {
      if (profile) {
        token.username = profile.user_name;
      }

      if (account) {
        token.access_token = account.access_token;
      }

      return token;
    },

    async session({ session, token }) {
      if (token) {
        session.user.username = token.username;
      }

      return session;
    },
  },
};
