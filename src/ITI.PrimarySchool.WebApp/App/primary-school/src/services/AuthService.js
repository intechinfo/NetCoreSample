class AuthService {
    constructor() {
        this.allowedOrigins = [];
        this.loginEndpoint = null;
        this.logoutEndpoint = null;
        this.appRedirect = () => null;

        window.addEventListener("message", this.onMessage, false);
    }

    get identity() {
        return ITI.PrimarySchool.getIdentity();
    }

    set identity(i) {
        ITI.PrimarySchool.setIdentity(i);
    }

    get isConnected() {
        return this.identity != null;
    }

    get accessToken() {
        var identity = this.identity;

        return identity ? identity.bearer.access_token : null;
    }

    get email() {
        var identity = this.identity;

        return identity ? identity.email : null;
    }

    onMessage = (e) => {
        if(this.allowedOrigins.indexOf(e.origin) < 0) return;

        var data = JSON.parse(e.data);

        if(data.type == 'authenticated') this.onAuthenticated(data.payload);
        else if(data.type == 'signedOut') this.onSignedOut();
        else throw new Error("Unknown message type");
    }

    login = () => {
        var popup = window.open(this.loginEndpoint, "Connexion à ITI.PrimarySchool", "menubar=no, status=no, scrollbars=no, menubar=no, width=500, height=300");
    }

    onAuthenticated = (i) => {
        this.identity = i;
        
        this.appRedirect();
    }

    logout = () => {
        var popup = window.open(this.logoutEndpoint, "Déconnexion d'ITI.PrimarySchool", "menubar=no, status=no, scrollbars=no, menubar=no, width=500, height=300");        
    }

    onSignedOut = () => {
        this.identity = null;

        this.appRedirect();        
    }
}

export default new AuthService();