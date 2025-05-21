<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="randomwheel.aspx.cs" Inherits="TrietPhamShopWeb.randomwheel" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Random Picker Wheel</title>
    <style>
        #wheelCanvas {
            border: 8px solid #444;
            border-radius: 50%;
        }
        #result {
            font-size: 24px;
            margin-top: 20px;
            font-weight: bold;
            color: darkgreen;
        }
        textarea {
            width: 100%;
            height: 100px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="text-align:center; padding:20px;">
            <h2>🎯 Picker Wheel</h2>
            <asp:TextBox ID="txtNames" runat="server" TextMode="MultiLine" Rows="6" placeholder="Nhập từng dòng một tên hoặc mục tiêu..."></asp:TextBox>
            <br /><br />
            <asp:Button ID="btnLoad" runat="server" Text="Tải vào bánh xe" OnClientClick="loadNames(); return false;" CssClass="btn btn-primary" />
            <button type="button" onclick="spinWheel()">🎡 Quay</button>
            <br /><br />
            <canvas id="wheelCanvas" width="400" height="400"></canvas>
            <div id="result"></div>
        </div>
    </form>

    <script>
        let names = [];
        let colors = [];
        let startAngle = 0;
        let arc;
        let spinTimeout = null;
        let spinAngleStart = 0;
        let spinTime = 0;
        let spinTimeTotal = 0;
        const ctx = document.getElementById("wheelCanvas").getContext("2d");

        function loadNames() {
            const raw = document.getElementById("<%= txtNames.ClientID %>").value;
            names = raw.split('\n').map(x => x.trim()).filter(x => x !== "");
            colors = names.map(() => "#" + Math.floor(Math.random()*16777215).toString(16));
            arc = Math.PI / (names.length / 2);
            drawWheel();
        }

        function drawWheel() {
            const canvas = document.getElementById("wheelCanvas");
            const outsideRadius = 150;
            const textRadius = 120;
            const insideRadius = 50;

            ctx.clearRect(0, 0, canvas.width, canvas.height);

            for (let i = 0; i < names.length; i++) {
                const angle = startAngle + i * arc;
                ctx.fillStyle = colors[i];

                ctx.beginPath();
                ctx.arc(200, 200, outsideRadius, angle, angle + arc, false);
                ctx.arc(200, 200, insideRadius, angle + arc, angle, true);
                ctx.fill();

                ctx.save();
                ctx.fillStyle = "white";
                ctx.translate(200 + Math.cos(angle + arc / 2) * textRadius,
                              200 + Math.sin(angle + arc / 2) * textRadius);
                ctx.rotate(angle + arc / 2);
                ctx.fillText(names[i], -ctx.measureText(names[i]).width / 2, 0);
                ctx.restore();
            }
        }

        function spinWheel() {
            spinAngleStart = Math.random() * 10 + 10;
            spinTime = 0;
            spinTimeTotal = Math.random() * 3000 + 4000;
            rotateWheel();
        }

        function rotateWheel() {
            spinTime += 30;
            if (spinTime >= spinTimeTotal) {
                stopRotateWheel();
                return;
            }
            const spinAngle = spinAngleStart - easeOut(spinTime, 0, spinAngleStart, spinTimeTotal);
            startAngle += (spinAngle * Math.PI / 180);
            drawWheel();
            spinTimeout = setTimeout(rotateWheel, 30);
        }

        function stopRotateWheel() {
            const degrees = startAngle * 180 / Math.PI + 90;
            const arcd = arc * 180 / Math.PI;
            const index = Math.floor((360 - (degrees % 360)) / arcd);
            document.getElementById("result").innerHTML = `🎉 Kết quả: <b>${names[index]}</b>`;
        }

        function easeOut(t, b, c, d) {
            const ts = (t /= d) * t;
            const tc = ts * t;
            return b + c * (tc + -3 * ts + 3 * t);
        }
    </script>
</body>
</html>