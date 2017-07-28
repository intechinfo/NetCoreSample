import $ from 'jquery'
import AuthService from '../services/AuthService'

function dataFilter(data, type) {
    if(data === '') return null;
    return data;
}

export async function postAsync(url, data) {
    return await $.ajax({
        method: 'POST',
        url: url,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(data),
        dataFilter: dataFilter,
        headers: {
            Authorization: `Bearer ${AuthService.accessToken}`
        }
    });
}

export async function putAsync(url, data) {
    return await $.ajax({
        method: 'PUT',
        url: url,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(data),
        dataFilter: dataFilter,
        headers: {
            Authorization: `Bearer ${AuthService.accessToken}`
        }
    });
}

export async function getAsync(url) {
    return await $.ajax({
        method: 'GET',
        url: url,
        dataType: 'json',
        dataFilter: dataFilter,
        headers: {
            Authorization: `Bearer ${AuthService.accessToken}`
        }
    });
}

export async function deleteAsync(url) {
    return await $.ajax({
        method: 'DELETE',
        url: url,
        dataType: 'json',
        dataFilter: dataFilter,
        headers: {
            Authorization: `Bearer ${AuthService.accessToken}`
        }
    });
}