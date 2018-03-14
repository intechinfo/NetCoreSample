import AuthService from '../services/AuthService'

async function checkErrors(resp) {
    if(resp.ok) return resp;

    let errorMsg = `ERROR ${resp.status} (${resp.statusText})`;
    let serverText = await resp.text();
    if(serverText) errorMsg = `${errorMsg}: ${serverText}`;

    var error = new Error(errorMsg);
    error.response = resp;
    throw error;
}

async function toJSON(resp) {
    const result = await resp.text();
    if(result) return JSON.parse(result);
}

export async function postAsync(url, data) {
    return await fetch(url, {
        method: 'POST',
        body: JSON.stringify(data),
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${AuthService.accessToken}`
        }
    })
    .then(checkErrors)
    .then(toJSON);
}

export async function putAsync(url, data) {
    return await fetch(url, {
        method: 'PUT',
        body: JSON.stringify(data),
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${AuthService.accessToken}`
        }
    })
    .then(checkErrors)
    .then(toJSON);
}

export async function getAsync(url) {
    return await fetch(url, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${AuthService.accessToken}`
        }
    })
    .then(checkErrors)
    .then(toJSON);
}

export async function deleteAsync(url) {
    return await fetch(url, {
        method: 'DELETE',
        headers: {
            'Authorization': `Bearer ${AuthService.accessToken}`
        }
    })
    .then(checkErrors)
    .then(toJSON);
}