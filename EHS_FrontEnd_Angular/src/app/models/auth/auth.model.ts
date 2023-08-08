export class AuthModel {
  access_token: string;
  refresh_token: string;
  expires_in: Date;
  roles: string[]

  // setAuth(auth: AuthModel) {
  //   this.authToken = auth.authToken;
  //   this.refreshToken = auth.refreshToken;
  //   this.expiresIn = auth.expiresIn;
  // }
}
