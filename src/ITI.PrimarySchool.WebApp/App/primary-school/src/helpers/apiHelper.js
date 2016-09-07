import $ from 'jquery'

export async function postAsync(endpoint, action, token, data) {
    return await $.ajax({
        method: 'POST',
        url: endpoint.concat('/', action),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(data),
        headers: {
            Authorization: `Bearer ${token}`
        }
    });
}

export async function putAsync(endpoint, action, token, data) {
    return await $.ajax({
        method: 'PUT',
        url: endpoint.concat('/', action),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(data),
        headers: {
            Authorization: `Bearer ${token}`
        }
    });
}

export async function getAsync(endpoint, action, token, data) {
    return await $.ajax({
        method: 'GET',
        url: endpoint.concat('/', action),
        dataType: 'json',
        data: data || {},
        headers: {
            Authorization: `Bearer ${token}`
        }
    });
}

export async function deleteAsync(endpoint, action, token, data) {
    return await $.ajax({
        method: 'DELETE',
        url: endpoint.concat('/', action),
        dataType: 'json',
        data: data || {},
        headers: {
            Authorization: `Bearer ${token}`
        }
    });
}