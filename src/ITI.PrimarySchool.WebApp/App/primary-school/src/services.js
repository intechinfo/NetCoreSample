let config = ITI.PrimarySchool;

import ClassApiService from './services/ClassApiService'
export const ClassApi = new ClassApiService(config.bearer.access_token);

import TeacherApiService from './services/TeacherApiService'
export const TeacherApi = new TeacherApiService(config.bearer.access_token);

import StudentApiService from './services/StudentApiService'
export const StudentApi = new StudentApiService(config.bearer.access_token);

import AuthService from './services/AuthService'
export const Auth = new AuthService(config.email);