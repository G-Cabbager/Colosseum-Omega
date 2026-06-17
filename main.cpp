#include <iostream>
#include <fstream>
#include <vector>
#include <string>
#include <cmath>
#include <cstdlib>
#include <windows.h>

// 基础结构，包含所有可能的字段
struct BaseEntity {
    std::string owner;
    std::string type;
    int health = 256;
    int x, y;
    int facing = 64;        // 单位使用，步兵固定
    int subPos = 1;         // 步兵站位 (1-3)，单位忽略
    std::string status = "Guard";
    std::string tag = "None";
    int veterancy;          // 经验等级：单位0-2，步兵可按0/100/200
    int group;              // 分组存储值 -1~9
    int layer;              // 步兵样例中是0，单位样例中是-1
    int reserved1 = 1;      // 固定1
    int reserved2 = 0;      // 固定0
};

// 向上取整除法
int ceilDiv(int a, int b) {
    return (a + b - 1) / b;
}

// 通用坐标生成（与单位相同，返回位置向量）
std::vector<std::pair<int, int>> generatePositions(
    bool isRightDirection,
    int cornerX, int cornerY,
    int rows, int total)
{
    std::vector<std::pair<int, int>> positions;
    if (rows <= 0 || total <= 0) return positions;

    int cols = ceilDiv(total, rows);
    int baseCount = total / rows;
    int remainder = total % rows;
    int startFullRow = (rows - remainder) / 2;

    int topLeftX, topLeftY;
    if (isRightDirection) {
        topLeftX = cornerX - (cols - 1);
        topLeftY = cornerY + (cols - 1);
    }
    else {
        topLeftX = cornerX - (rows - 1);
        topLeftY = cornerY - (rows - 1);
    }

    const int dcX = 1;
    const int dcY = -1;

    for (int r = 0; r < rows; ++r) {
        int numInRow = baseCount;
        if (r >= startFullRow && r < startFullRow + remainder)
            numInRow = baseCount + 1;
        if (numInRow == 0) continue;

        int startCol, endCol;
        if (isRightDirection) {
            startCol = cols - numInRow;
            endCol = cols;
        }
        else {
            startCol = 0;
            endCol = numInRow;
        }

        int rowBaseX = topLeftX + r;
        int rowBaseY = topLeftY + r;

        for (int c = startCol; c < endCol; ++c) {
            int x = rowBaseX + c * dcX;
            int y = rowBaseY + c * dcY;
            positions.push_back({ x, y });
        }
    }
    return positions;
}

// 生成单位列表 (Units)
std::vector<BaseEntity> generateUnitsList(
    bool isRightDirection,
    int cornerX, int cornerY,
    int rows, int total,
    const BaseEntity& base)
{
    auto positions = generatePositions(isRightDirection, cornerX, cornerY, rows, total);
    std::vector<BaseEntity> entities;
    for (const auto& pos : positions) {
        BaseEntity e = base;
        e.x = pos.first;
        e.y = pos.second;
        // 单位使用的特殊默认值
        e.layer = -1;      // 单位样例中 layer 为 -1
        entities.push_back(e);
    }
    return entities;
}

// 生成步兵列表 (Infantry)
// 步兵在同一格子有三个站位，但本程序要求用户指定总数量（总步兵数），
// 并且每个步兵独立使用一个格子（一个格子一个步兵），
// 如果需要三个站位堆叠，用户应该设置总数量为3的倍数，
// 并且生成三个独立的步兵单元，每个具有相同的坐标但站位不同。
// 为简化，本函数按总数量生成，每个步兵都有自己的坐标（不堆叠）。
// 如果用户希望堆叠，可以多次运行程序并手动合并。
std::vector<BaseEntity> generateInfantryList(
    bool isRightDirection,
    int cornerX, int cornerY,
    int rows, int total,
    const BaseEntity& base)
{
    auto positions = generatePositions(isRightDirection, cornerX, cornerY, rows, total);
    std::vector<BaseEntity> entities;
    for (const auto& pos : positions) {
        BaseEntity e = base;
        e.x = pos.first;
        e.y = pos.second;
        // 步兵使用的特殊默认值
        e.layer = 0;        // 步兵样例中 layer 为 0
        entities.push_back(e);
    }
    return entities;
}

// 输出 Units 区块
void writeUnitsBlock(std::ofstream& out, const std::vector<BaseEntity>& units, int startIndex) {
    out << "[Units]\n";
    for (size_t i = 0; i < units.size(); ++i) {
        const auto& u = units[i];
        out << (startIndex + i) << "="
            << u.owner << ","
            << u.type << ","
            << u.health << ","
            << u.x << ","
            << u.y << ","
            << u.facing << ","
            << u.status << ","
            << u.tag << ","
            << 0 << ","           // waypoint 固定0
            << u.group << ","
            << 0 << ","           // trigger 固定0
            << u.layer << ","
            << u.veterancy << ","
            << u.reserved2 << "\n";
    }
}

// 输出 Infantry 区块
void writeInfantryBlock(std::ofstream& out, const std::vector<BaseEntity>& infantry, int startIndex) {
    out << "[Infantry]\n";
    for (size_t i = 0; i < infantry.size(); ++i) {
        const auto& inf = infantry[i];
        out << (startIndex + i) << "="
            << inf.owner << ","
            << inf.type << ","
            << inf.health << ","
            << inf.x << ","
            << inf.y << ","
            << inf.subPos << ","
            << inf.status << ","
            << inf.facing << ","
            << inf.tag << ","
            << inf.veterancy << ","
            << inf.group << ","
            << inf.layer << ","
            << inf.reserved1 << ","
            << inf.reserved2 << "\n";
    }
}

int main() {
    // 设置控制台输出为 UTF-8 编码，防止中文乱码
    SetConsoleOutputCP(CP_UTF8);

    std::cout << "=== RA2 单位/步兵批量生成工具 ===\n";
    std::cout << "请选择生成类型:\n";
    std::cout << "1 - 单位 (Vehicle/Unit)\n";
    std::cout << "2 - 步兵 (Infantry)\n";
    int choice;
    std::cin >> choice;
    std::cin.ignore();

    BaseEntity base;

    // 通用输入
    std::cout << "所属方 (如 UnitedStates House): ";
    std::getline(std::cin, base.owner);
    std::cout << "类型代码 (单位如 MTNK, 步兵如 E1): ";
    std::getline(std::cin, base.type);

    if (choice == 1) {
        // 单位特有输入
        std::cout << "朝向 (0-255, 默认64): ";
        std::cin >> base.facing;
        std::cout << "经验等级 (0=新兵,1=老兵,2=精英): ";
        std::cin >> base.veterancy;
        std::cout << "分组存储值 (-1=无分组, 0~9 对应游戏显示 1~10): ";
        std::cin >> base.group;
        // 单位 layer 固定 -1，稍后在生成时设置
    }
    else {
        // 步兵特有输入
        std::cout << "站位 (1,2,3 统一值): ";
        std::cin >> base.subPos;
        std::cout << "经验等级 (通常 0,100,200): ";
        std::cin >> base.veterancy;
        std::cout << "分组存储值 (-1=无分组, 0~9 对应游戏显示 1~10): ";
        std::cin >> base.group;
        // 步兵的 facing 固定64，不需要输入
        base.facing = 64;
    }

    // 生成参数（共用）
    int dir;
    std::cout << "生成方向 (0=左向, 1=右向): ";
    std::cin >> dir;
    bool isRight = (dir == 1);
    int cornerX, cornerY;
    std::cout << "角落坐标 X: ";
    std::cin >> cornerX;
    std::cout << "角落坐标 Y: ";
    std::cin >> cornerY;
    int rows, total;
    std::cout << "行数: ";
    std::cin >> rows;
    std::cout << "总单位/步兵数量: ";
    std::cin >> total;
    int startIndex;
    std::cout << "起始序号 (例如 0): ";
    std::cin >> startIndex;

    // 打开输出文件
    std::ofstream outFile("MAPS/test.map");
    if (!outFile.is_open()) {
        std::cerr << "错误: 无法创建文件 MAPS/test.map\n";
        return 1;
    }

    // 根据类型生成
    if (choice == 1) {
        auto units = generateUnitsList(isRight, cornerX, cornerY, rows, total, base);
        writeUnitsBlock(outFile, units, startIndex);
        std::cout << "\n已生成 [Units] 区块并写入 MAPS/test.map\n";
    }
    else {
        auto infantry = generateInfantryList(isRight, cornerX, cornerY, rows, total, base);
        writeInfantryBlock(outFile, infantry, startIndex);
        std::cout << "\n已生成 [Infantry] 区块并写入 MAPS/test.map\n";
    }

    outFile.close();
    return 0;
}
