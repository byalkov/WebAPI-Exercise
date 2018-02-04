API Endpoints:

POST /token - get Token Auth generated
GET /api/customer - get current customer using token auth SID
GET /api/customer/{id}  - get customer by id
GET /api/order/{orderid} - get order by id
GET /api/order/{startDate-endDate} - get orders from between 2 dates
GET /api/order/awaiting - get orders in AwaitingDispatch state
GET /api/shoppingCart - get current customer's Shopping cart
POST /api/shoppingCart/add/{productId}/{quantity?} - add a product with optional quantity (default =1) to current customer's shopping cart
PUT /api/shoppingCart/update/{productId}/{quantity} - update a product in the current customer's shopping cart. Item will be removed if the quantity is reduced to 0
POST /api/shoppingCart/buy - purchase current customer's shopping cart


Assumptions:
- Data is only kept during runtime and is not persistently stored.
- Repository pattern is simplified.
- Not all CRUD methods are required (some are out of scope).
- No generics pattern applied to the Repository class (out of scope).
- APIs and Services only implement the required endpoints.
- Security model is simplified and no roles based security model is needed.
- Documenting the API Endpoints with Swagger is out of scope.
- Exception handling for invalid requests is out of scope.
- Using External Ids in DTO objects is out of scope

Other assumptions included as comments where relevant. 