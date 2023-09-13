using Microsoft.AspNetCore.SignalR;

namespace Go_Cart
{
    public class ReviewsHub : Hub
    {
        public async Task SendReview(string review)
        {
            // Save the review in your database or perform any necessary processing

            // Send the review to all connected clients
            await Clients.All.SendAsync("ReceiveReview", review);
        }
    }
}
