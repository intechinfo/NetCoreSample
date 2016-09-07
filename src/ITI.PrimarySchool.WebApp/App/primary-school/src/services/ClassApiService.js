import { getAsync, postAsync, putAsync, deleteAsync } from '../helpers/apiHelper'

const endpoint = "/api/classes";

class ClassApiService {

  constructor(token) {
      this.token = token;
  }

  async getClassListAsync() {
      return await getAsync(endpoint, 'GetClassList', this.token);
  }

  async getClassAsync(classId) {
      return await getAsync(endpoint, 'GetClassById', this.token, { classId: classId });
  }

  async createClassAsync(model) {
      return await postAsync(endpoint, 'CreateClass', this.token, { model: model });
  }

  async updateClassAsync(model) {
      return await putAsync(endpoint, 'UpdateClass', this.token, { model: model });
  }

  async deleteClassAsync(classId) {
      return await deleteAsync(endpoint, 'DeleteClass', this.token, { classId: classId });
  }
}

export default ClassApiService