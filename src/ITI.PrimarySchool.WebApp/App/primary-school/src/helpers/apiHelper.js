import $ from 'jquery'

function dataFilter(data, type) {
    if(data === '') return null;
    return data;
}

export async function postAsync(endpoint, id, token, data) {
    return await $.ajax({
        method: 'POST',
        url: endpoint.concat('/', id),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(data),
        dataFilter: dataFilter,
        headers: {
            Authorization: `Bearer ${token}`
        }
    });
}

export async function putAsync(endpoint, id, token, data) {
    return await $.ajax({
        method: 'PUT',
        url: endpoint.concat('/', id),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(data),
        dataFilter: dataFilter,
        headers: {
            Authorization: `Bearer ${token}`
        }
    });
}

export async function getAsync(endpoint, id, token) {
    return await $.ajax({
        method: 'GET',
        url: endpoint.concat('/', id),
        dataType: 'json',
        dataFilter: dataFilter,
        headers: {
            Authorization: `Bearer ${token}`
        }
    });
}

export async function deleteAsync(endpoint, id, token) {
    return await $.ajax({
        method: 'DELETE',
        url: endpoint.concat('/', id),
        dataType: 'json',
        dataFilter: dataFilter,
        headers: {
            Authorization: `Bearer ${token}`
        }
    });
}