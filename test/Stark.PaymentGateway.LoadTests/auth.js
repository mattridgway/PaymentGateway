import http from "k6/http";

let _authToken;

export let authenticate = function () {

    if (_authToken) {
        return _authToken;
    }

    const authorisationData = {
        client_id: 'sampleapplication',
        client_secret: 'secret',
        grant_type: 'client_credentials',
    };

    const authUrl = __ENV.AUTHURL || "https://localhost:5101";

    const tokenResponse = http.post(`${authUrl}/connect/token`, authorisationData);
    const tokenResponseJson = tokenResponse.json();
    _authToken = tokenResponseJson.access_token;

    return _authToken;
};