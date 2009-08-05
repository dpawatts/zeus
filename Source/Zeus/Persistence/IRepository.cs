namespace Zeus.Persistence
{
	public interface IRepository<TKey, TEntity>
	{
		ITransaction BeginTransaction();
		void Flush();

		/// <summary>
		/// Register the entity for deletion when the unit of work
		/// is completed. 
		/// </summary>
		/// <param name="entity">The entity to delete</param>
		void Delete(TEntity entity);

		/// <summary>
		/// Get the entity from the persistance store, or return null
		/// if it doesn't exist.
		/// </summary>
		/// <param name="id">The entity's id</param>
		/// <returns>Either the entity that matches the id, or a null</returns>
		TEntity Get(TKey id);

		/// <summary>
		/// Get the entity from the persistance store, or return null
		/// if it doesn't exist.
		/// </summary>
		/// <param name="id">The entity's id</param>
		/// <typeparam name="T">The type of entity to get.</typeparam>
		/// <returns>Either the entity that matches the id, or a null</returns>
		T Get<T>(TKey id)
			where T : TEntity;
		
		/// <summary>
		/// Load the entity from the persistance store
		/// Will throw an exception if there isn't an entity that matches
		/// the id.
		/// </summary>
		/// <param name="id">The entity's id</param>
		/// <returns>The entity that matches the id</returns>
		TEntity Load(TKey id);

		/// <summary>
		/// Register te entity for save in the database when the unit of work
		/// is completed. (INSERT)
		/// </summary>
		/// <param name="entity">the entity to save</param>
		void Save(TEntity entity);

		/// <summary>
		/// Register te entity for save or update in the database when the unit of work
		/// is completed. (INSERT or UPDATE)
		/// </summary>
		/// <param name="entity">the entity to save</param>
		void SaveOrUpdate(TEntity entity);

		/// <summary>
		/// Register the entity for update in the database when the unit of work
		/// is completed. (UPDATE)
		/// </summary>
		/// <param name="entity"></param>
		void Update(TEntity entity);
	}
}
