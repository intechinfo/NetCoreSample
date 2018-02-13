class AuthService {
    constructor() {
        this.allowedOrigins = [];
        this.providers = {};
        this.logoutEndpoint = null;
        this.appRedirect = () => null;
        this.authenticatedCallbacks = [];
        this.signedOutCallbacks = [];

        window.addEventListener("message", (e) => this.onMessage(e), false);
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

    get boundProviders() {
        var identity = this.identity;

        return identity ? identity.boundProviders : [];
    }

    isBoundToProvider(expectedProviders) {
        var isBound = false;

        for (var p of expectedProviders) {
            if (this.boundProviders.indexOf(p) > -1) isBound = true;
        }

        return isBound;
    }

    onMessage(e) {
        if (!e.origin || this.allowedOrigins.indexOf(e.origin) < 0) return;

        var data = e.data;

        if (data.type == 'authenticated') this.onAuthenticated(data.payload);
        else if (data.type == 'signedOut') this.onSignedOut();
    }

    login(selectedProvider) {
        var provider = this.providers[selectedProvider];

        var popup = window.open(provider.endpoint, "Connexion à ITI.PrimarySchool", "menubar=no, status=no, scrollbars=no, menubar=no, width=700, height=700");
    }

    registerAuthenticatedCallback(cb) {
        this.authenticatedCallbacks.push(cb);
    }

    removeAuthenticatedCallback(cb) {
        this.authenticatedCallbacks.splice(this.authenticatedCallbacks.indexOf(cb), 1);
    }

    onAuthenticated(i) {
        this.identity = i;

        for (var cb of this.authenticatedCallbacks) {
            cb();
        }
    }

    logout() {
        var popup = window.open(this.logoutEndpoint, "Déconnexion d'ITI.PrimarySchool", "menubar=no, status=no, scrollbars=no, menubar=no, width=700, height=600");
    }

    registerSignedOutCallback(cb) {
        this.signedOutCallbacks.push(cb);
    }

    removeSignedOutCallback(cb) {
        this.signedOutCallbacks.splice(this.signedOutCallbacks.indexOf(cb), 1);
    }

    onSignedOut() {
        this.identity = null;

        for (var cb of this.signedOutCallbacks) {
            cb();
        }
    }
}

export default new AuthService();