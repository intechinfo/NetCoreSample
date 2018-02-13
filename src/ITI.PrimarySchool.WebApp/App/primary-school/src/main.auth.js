import AuthService from './services/AuthService'

/**
 * Configuration of the authentication service
 */

// Allowed urls to access the application (if your website is http://mywebsite.com, you have to add it)
AuthService.allowedOrigins = ['http://localhost:5000', /* 'http://mywebsite.com' */];

// Server-side endpoint to logout
AuthService.logoutEndpoint = '/Account/LogOff';

// Allowed providers to log in our application, and the corresponding server-side endpoints
AuthService.providers = {
  'Base': {
    endpoint: '/Account/Login'
  },
  'Google': {
    endpoint: '/Account/ExternalLogin?provider=Google'
  },
  'GitHub': {
    endpoint: '/Account/ExternalLogin?provider=GitHub'
  },
};

AuthService.appRedirect = () => router.replace('/');